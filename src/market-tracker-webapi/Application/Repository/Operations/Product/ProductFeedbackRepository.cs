using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

public class ProductFeedbackRepository(MarketTrackerDataContext dataContext) : IProductFeedbackRepository
{
    public async Task<PaginatedResult<ProductReview>> GetReviewsByProductIdAsync(string productId, int skip, int take)
    {
        var query = dataContext
            .ProductReview
            .Where(review => review.ProductId == productId)
            .OrderByDescending(review => review.CreatedAt)
            .Skip(skip)
            .Take(take)
            .Select(productReviewEntity => productReviewEntity.ToProductReview());

        var reviews = await query.ToListAsync();

        return new PaginatedResult<ProductReview>(reviews, query.Count(), skip, take);
    }

    public async Task<ProductReview> AddReviewAsync(
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
        return productReviewEntity.ToProductReview();
    }

    public async Task<ProductReview?> UpsertReviewAsync(
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
            return await AddReviewAsync(clientId, productId, rating, comment);
        }

        reviewEntity.Rating = rating;
        reviewEntity.Text = comment;

        await dataContext.SaveChangesAsync();
        return reviewEntity.ToProductReview();
    }

    public async Task<ProductReview?> RemoveReviewAsync(Guid clientId, string productId)
    {
        var reviewEntity = await dataContext.ProductReview.FindAsync(clientId, productId);
        if (reviewEntity is null)
        {
            return null;
        }

        dataContext.ProductReview.Remove(reviewEntity);

        await dataContext.SaveChangesAsync();
        return reviewEntity.ToProductReview();
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

    public async Task<ProductPreferences> GetProductsPreferencesAsync(
        Guid clientId,
        string productId
    )
    {
        var isFavourite = await dataContext.ProductFavorite.AnyAsync(favourite =>
            favourite.ProductId == productId && favourite.ClientId == clientId
        );

        var productReview = await dataContext
            .ProductReview.Where(review =>
                review.ProductId == productId && review.ClientId == clientId
            )
            .Select(review => review.ToProductReview())
            .FirstOrDefaultAsync();

        return new ProductPreferences(isFavourite, productReview);
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