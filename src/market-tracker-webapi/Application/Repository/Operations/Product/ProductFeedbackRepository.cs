using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

public class ProductFeedbackRepository(MarketTrackerDataContext dataContext)
    : IProductFeedbackRepository
{
    public async Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(int productId)
    {
        return await dataContext
            .ProductReview.Where(review => review.ProductId == productId)
            .OrderByDescending(review => review.CreatedAt)
            .Select(productReviewEntity => productReviewEntity.ToProductReview())
            .ToListAsync();
    }

    public async Task<int> AddReviewAsync(Guid clientId, int productId, int rate, string? comment)
    {
        var productReviewEntity = new ProductReviewEntity()
        {
            ProductId = productId,
            ClientId = clientId,
            Rating = rate,
            Text = comment
        };
        await dataContext.ProductReview.AddAsync(productReviewEntity);
        await dataContext.SaveChangesAsync();
        return productReviewEntity.Id;
    }

    public async Task<ProductReview?> UpsertReviewAsync(
        Guid clientId,
        int productId,
        int rate,
        string? comment
    )
    {
        var reviewEntity = new ProductReviewEntity()
        {
            ProductId = productId,
            ClientId = clientId,
            Rating = rate,
            Text = comment
        };
        await dataContext.ProductReview.Upsert(reviewEntity).RunAsync();
        await dataContext.SaveChangesAsync();
        return reviewEntity.ToProductReview();
    }

    public async Task<ProductReview?> RemoveReviewAsync(Guid clientId, int productId)
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
        int productId,
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

    public async Task<PriceAlert?> RemovePriceAlertAsync(Guid clientId, int productId)
    {
        var priceAlertEntity = dataContext.PriceAlert.Find(clientId, productId);
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
        int productId,
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

    public async Task<ProductPreferences> GetUserFeedbackByProductId(Guid clientId, int productId)
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

    public async Task<ProductStats?> GetProductStatsByIdAsync(int productId)
    {
        return await dataContext
            .ProductStatsCounts.Where(stats => stats.ProductId == productId)
            .Select(stats => new ProductStats(
                productId,
                new ProductStatsCounts(stats.Favourites, stats.Ratings, stats.Lists),
                0 // TODO: leave on product table or in stats table?
            ))
            .FirstOrDefaultAsync();
    }
}
