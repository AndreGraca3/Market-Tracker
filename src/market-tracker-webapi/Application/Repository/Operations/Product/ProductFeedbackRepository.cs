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

    public async Task<int> AddReviewAsync(
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
        return productReviewEntity.Id;
    }

    public async Task<ProductReview?> UpsertReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    )
    {
        var reviewEntity = new ProductReviewEntity()
        {
            ProductId = productId,
            ClientId = clientId,
            Rating = rating,
            Text = comment
        };
        await dataContext.ProductReview.Upsert(reviewEntity).RunAsync();
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

    public async Task<PriceAlert> UpsertPriceAlertAsync(
        Guid clientId,
        string productId,
        int priceThreshold
    )
    {
        var priceAlertEntity = new PriceAlertEntity()
        {
            ProductId = productId,
            ClientId = clientId,
            PriceThreshold = priceThreshold
        };
        await dataContext.PriceAlert.Upsert(priceAlertEntity).RunAsync();
        await dataContext.SaveChangesAsync();
        return priceAlertEntity.ToPriceAlert();
    }

    public async Task<PriceAlert?> RemovePriceAlertAsync(Guid clientId, string productId)
    {
        var priceAlertEntity = await dataContext.PriceAlert.FindAsync(clientId, productId);
        if (priceAlertEntity is null)
        {
            return null;
        }

        dataContext.PriceAlert.Remove(priceAlertEntity);
        await dataContext.SaveChangesAsync();
        return priceAlertEntity.ToPriceAlert();
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

        var priceAlert = await dataContext
            .PriceAlert.Where(alert => alert.ProductId == productId && alert.ClientId == clientId)
            .Select(alert => alert.ToPriceAlert())
            .FirstOrDefaultAsync();

        var productReview = await dataContext
            .ProductReview.Where(review =>
                review.ProductId == productId && review.ClientId == clientId
            )
            .Select(review => review.ToProductReview())
            .FirstOrDefaultAsync();

        return new ProductPreferences(isFavourite, priceAlert, productReview);
    }

    public async Task<ProductStats?> GetProductStatsByIdAsync(string productId)
    {
        return await dataContext
            .ProductStatsCounts.Where(stats => stats.ProductId == productId)
            .Join(
                dataContext.Product,
                stats => stats.ProductId,
                product => product.Id,
                (stats, product) =>
                    new ProductStats(
                        productId,
                        new ProductStatsCounts(stats.Favourites, stats.Ratings, stats.Lists),
                        product.Rating
                    )
            )
            .FirstOrDefaultAsync();
    }
}