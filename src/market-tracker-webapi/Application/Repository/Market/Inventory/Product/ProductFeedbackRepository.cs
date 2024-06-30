using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Inventory.Product;

public class ProductFeedbackRepository(MarketTrackerDataContext dataContext) : IProductFeedbackRepository
{
    public async Task<PaginatedResult<ProductReview>> GetReviewsByProductIdAsync(string productId, int skip, int take)
    {
        var query = from pr in dataContext.ProductReview
            join client in dataContext.Client on pr.ClientId equals client.UserId
            where pr.ProductId == productId
            select new
            {
                ProductReviewEntity = pr,
                ClientEntity = client
            };

        var reviews = await query
            .OrderByDescending(p => p.ProductReviewEntity.CreatedAt)
            .Skip(skip)
            .Take(take)
            .Select(p =>
                p.ProductReviewEntity.ToProductReview(
                    p.ClientEntity.ToClientItem()
                )).ToListAsync();

        return new PaginatedResult<ProductReview>(reviews, query.Count(), skip, take);
    }

    public async Task<ProductReview?> GetReviewByIdAsync(int reviewId)
    {
        var query = from pr in dataContext.ProductReview
            join client in dataContext.Client on pr.ClientId equals client.UserId
            where pr.Id == reviewId
            select new
            {
                ProductReviewEntity = pr,
                ClientEntity = client
            };

        return await query
            .Select(p =>
                p.ProductReviewEntity.ToProductReview(
                    p.ClientEntity.ToClientItem()
                )).FirstOrDefaultAsync();
    }

    public async Task<ReviewId> AddReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    )
    {
        var productReviewEntity = new ProductReviewEntity
        {
            ProductId = productId,
            ClientId = clientId,
            Rating = rating,
            Text = comment,
            CreatedAt = DateTime.UtcNow
        };
        await dataContext.ProductReview.AddAsync(productReviewEntity);
        await dataContext.SaveChangesAsync();
        return new ReviewId(productReviewEntity.Id);
    }

    public async Task<ProductReview?> UpdateReviewAsync(
        int reviewId,
        int rating,
        string? comment
    )
    {
        var reviewEntity = await dataContext.ProductReview.FirstOrDefaultAsync(review => review.Id == reviewId);

        if (reviewEntity is null)
        {
            return null;
        }

        reviewEntity.Rating = rating;
        reviewEntity.Text = comment;

        await dataContext.SaveChangesAsync();

        var client = await dataContext.Client.FindAsync(reviewEntity.ClientId);
        return client is null ? null : reviewEntity.ToProductReview(client.ToClientItem());
    }

    public async Task<ProductReview?> RemoveReviewAsync(int reviewId)
    {
        var reviewEntity = await dataContext.ProductReview.FindAsync(reviewId);
        if (reviewEntity is null)
        {
            return null;
        }

        var review = (await GetProductPreferencesAsync(reviewEntity.ClientId, reviewEntity.ProductId)).Review;
        dataContext.ProductReview.Remove(reviewEntity);
        await dataContext.SaveChangesAsync();
        return review;
    }

    public async Task<IEnumerable<ProductItem>> GetFavouriteProductsAsync(Guid clientId)
    {
        var query = from productFavorite in dataContext.ProductFavorite
            join product in dataContext.Product on productFavorite.ProductId equals product.Id
            join brand in dataContext.Brand on product.BrandId equals brand.Id 
            where productFavorite.ClientId == clientId
            select new
            {
                ProductEntity = product,
                BrandEntity = brand
            };

        return await query
            .Select(g =>
                g.ProductEntity.ToProductItem(g.BrandEntity.ToBrand())
            ).ToListAsync();
    }

    public async Task<bool> UpdateProductFavouriteAsync(
        Guid clientId,
        string productId,
        bool isFavourite
    )
    {
        var productFavoriteEntity = new ProductFavouriteEntity()
        {
            ProductId = productId,
            ClientId = clientId
        };
        if (isFavourite)
        {
            await dataContext.ProductFavorite.AddAsync(productFavoriteEntity);
        }
        else
        {
            dataContext.ProductFavorite.Remove(productFavoriteEntity);
        }

        await dataContext.SaveChangesAsync();
        return isFavourite;
    }

    public async Task<ProductPreferences> GetProductPreferencesAsync(Guid clientId, string productId)
    {
        var isFavourite = await dataContext.ProductFavorite.AnyAsync(favourite =>
            favourite.ProductId == productId && favourite.ClientId == clientId
        );

        var query = from productReview in dataContext.ProductReview
            join client in dataContext.Client on productReview.ClientId equals client.UserId
            where productReview.ProductId == productId && productReview.ClientId == clientId
            select new
            {
                ProductReviewEntity = productReview,
                clientEntity = client
            };

        return new ProductPreferences(
            isFavourite,
            await query
                .Select(g =>
                    g.ProductReviewEntity.ToProductReview(g.clientEntity.ToClientItem()))
                .FirstOrDefaultAsync()
        );
    }

    public async Task<ProductStats?> GetProductStatsByIdAsync(string productId)
    {
        return await (from stats in dataContext.ProductStatsCounts
                where stats.ProductId == productId
                join product in dataContext.Product on stats.ProductId equals product.Id
                select new ProductStats(
                    productId,
                    new ProductStatsCounts(stats.Favourites, stats.Ratings),
                    product.Rating
                )
            ).FirstOrDefaultAsync();
    }
}