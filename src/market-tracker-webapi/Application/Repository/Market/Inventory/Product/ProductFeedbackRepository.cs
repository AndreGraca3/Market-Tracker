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
        var query = from productReview in dataContext.ProductReview
            join client in dataContext.Client on productReview.ClientId equals client.UserId
            where productReview.ProductId == productId
            select new
            {
                ProductReviewEntity = productReview,
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

    public async Task<ReviewId> AddReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    )
    {
        var productReviewEntity = new ProductReviewEntity()
        {
            ProductId = productId,
            ClientId = clientId,
            Rating = rating,
            Text = comment
        };
        await dataContext.ProductReview.AddAsync(productReviewEntity);
        await dataContext.SaveChangesAsync();
        return new ReviewId(productReviewEntity.Id);
    }

    public async Task<ProductReview?> UpdateReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    )
    {
        var reviewEntity = await dataContext.ProductReview.FirstOrDefaultAsync(review =>
            review.ProductId == productId && review.ClientId == clientId
        );

        if (reviewEntity is null)
        {
            return null;
        }

        reviewEntity.Rating = rating;
        reviewEntity.Text = comment;

        await dataContext.SaveChangesAsync();
        return (await GetProductPreferencesAsync(clientId, productId)).Review;
    }

    public async Task<ProductReview> UpsertReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    )
    {
        var reviewEntity = await dataContext.ProductReview.FindAsync(clientId, productId);
        if (reviewEntity is null)
        {
            await AddReviewAsync(clientId, productId, rating, comment);
            return (await GetProductPreferencesAsync(clientId, productId)).Review!;
        }

        reviewEntity.Rating = rating;
        reviewEntity.Text = comment;
        await dataContext.SaveChangesAsync();
        return (await GetProductPreferencesAsync(clientId, productId)).Review!;
    }

    public async Task<ProductReview?> RemoveReviewAsync(Guid clientId, string productId)
    {
        var reviewEntity = await dataContext.ProductReview.FindAsync(clientId, productId);
        if (reviewEntity is null)
        {
            return null;
        }

        var review = (await GetProductPreferencesAsync(clientId, productId)).Review;
        dataContext.ProductReview.Remove(reviewEntity);
        await dataContext.SaveChangesAsync();
        return review;
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

    public async Task<ProductPreferences> GetProductPreferencesAsync(
        Guid clientId,
        string productId
    )
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
                .Select(g => g.ProductReviewEntity.ToProductReview(g.clientEntity.ToClientItem()))
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