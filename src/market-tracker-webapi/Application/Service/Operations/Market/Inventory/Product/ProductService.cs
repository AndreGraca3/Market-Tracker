using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Market.Alert;
using market_tracker_webapi.Application.Repository.Market.Inventory.Brand;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.External;
using market_tracker_webapi.Application.Service.Results;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

using Product = Domain.Schemas.Market.Inventory.Product.Product;

public class ProductService(
    IProductRepository productRepository,
    IBrandRepository brandRepository,
    ICategoryRepository categoryRepository,
    IPriceRepository priceRepository,
    IStoreRepository storeRepository,
    IClientDeviceRepository clientDeviceRepository,
    IPriceAlertRepository priceAlertRepository,
    INotificationService notificationService,
    ITransactionManager transactionManager
) : IProductService
{
    public async Task<PaginatedProductOffers> GetBestAvailableProductsOffersAsync(int skip,
        int take, int maxValuesPerFacet, ProductsSortOption? sortBy, string? searchName, IList<int>? categoryIds,
        IList<int>? brandIds, IList<int>? companyIds, int? minPrice, int? maxPrice, int? minRating, int? maxRating)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var availableProducts = await productRepository.GetAvailableProductsAsync(skip, take, maxValuesPerFacet,
                sortBy, searchName, categoryIds, brandIds, minPrice, maxPrice, minRating, maxRating, companyIds);

            var bestOffers = new List<ProductOffer>();

            foreach (var product in availableProducts.PaginatedProducts.Items)
            {
                var cheapestOffer =
                    await priceRepository.GetCheapestStoreOfferAvailableByProductIdAsync(product.Id.Value, companyIds);
                if (cheapestOffer is not null)
                {
                    bestOffers.Add(new ProductOffer(product, cheapestOffer));
                }
            }

            return new PaginatedProductOffers(
                new PaginatedResult<ProductOffer>(bestOffers, availableProducts.PaginatedProducts.TotalItems, skip,
                    take),
                availableProducts.FacetsCounters
            );
        });
    }

    public async Task<Product> GetProductByIdAsync(string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await productRepository.GetProductByIdAsync(productId) ?? throw new MarketTrackerServiceException(
                new ProductFetchingError.ProductByIdNotFound(productId)
            ));
    }

    public async Task<ProductCreationResult> AddProductAsync(
        Guid operatorId,
        string productId,
        string name,
        string imageUrl,
        int quantity,
        ProductUnit unit,
        string brandName,
        int categoryId,
        int basePrice,
        int? promotionPercentage
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var oldProduct = await productRepository.GetProductByIdAsync(productId);
            if (oldProduct is null)
            {
                if (await categoryRepository.GetCategoryByIdAsync(categoryId) is null)
                {
                    throw new MarketTrackerServiceException(
                        new CategoryFetchingError.CategoryByIdNotFound(categoryId)
                    );
                }

                var brand = await brandRepository.GetBrandByNameAsync(brandName)
                            ?? await brandRepository.AddBrandAsync(brandName);

                await productRepository.AddProductAsync(
                    productId,
                    $"{brandName} {name}" + (quantity == 1 && unit == ProductUnit.Units
                        ? ""
                        : $" {quantity}{unit.GetBaseUnitName()}"),
                    imageUrl,
                    quantity,
                    unit.GetUnitName(),
                    brand.Id,
                    categoryId
                );
            }

            var store = await storeRepository.GetStoreByOperatorIdAsync(operatorId);

            if (store is null)
            {
                throw new MarketTrackerServiceException(
                    new StoreFetchingError.StoreByOperatorIdNotFound(operatorId)
                );
            }

            var oldStoreOffer = oldProduct is null
                ? null
                : await priceRepository.GetStoreOfferAsync(productId, store.Id.Value, DateTime.UtcNow);

            var newPrice = basePrice.ApplyPercentage(promotionPercentage);
            var priceChanged = oldStoreOffer?.PriceData.FinalPrice != newPrice;

            if (priceChanged)
            {
                await priceRepository.AddPriceAsync(productId, store.Id.Value, basePrice, promotionPercentage);

                // Notify clients with price alerts
                var eligiblePriceAlerts =
                    await priceAlertRepository.GetPriceAlertsAsync(productId: productId, storeId: store.Id.Value,
                        minPriceThreshold: newPrice);
                foreach (var priceAlert in eligiblePriceAlerts)
                {
                    var clientTokens =
                        await clientDeviceRepository.GetDeviceTokensByClientIdAsync(priceAlert.ClientId);

                    await notificationService.SendNotificationToTokensAsync(
                        "Alerta de preço",
                        $"{oldProduct?.Name ?? name} custa agora {newPrice.CentsToEuros()}€ na loja {store.Name}",
                        clientTokens.Select(dT => dT.Token).ToList()
                    );
                }
            }

            await productRepository.SetProductAvailabilityAsync(productId, store.Id.Value, true);

            return new ProductCreationResult
            {
                Id = productId,
                IsNew = oldProduct is null,
                PriceChanged = priceChanged
            };
        });
    }

    public async Task<ProductId> SetProductAvailabilityAsync(
        Guid operatorId, string productId, bool isAvailable)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var store = await storeRepository.GetStoreByOperatorIdAsync(operatorId);
            if (store is null)
            {
                throw new MarketTrackerServiceException(
                    new StoreFetchingError.StoreByOperatorIdNotFound(operatorId)
                );
            }

            var product = await productRepository.GetProductByIdAsync(productId);
            if (product is null)
            {
                throw new MarketTrackerServiceException(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            await productRepository.SetProductAvailabilityAsync(productId, store.Id.Value, isAvailable);

            return product.Id;
        });
    }

    public async Task<Product> UpdateProductAsync(
        string productId,
        string? name,
        string? imageUrl,
        int? quantity,
        ProductUnit? unit,
        string? brandName,
        int? categoryId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product is null)
            {
                throw new MarketTrackerServiceException(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var brand = brandName is not null
                ? await brandRepository.GetBrandByNameAsync(brandName)
                  ?? await brandRepository.AddBrandAsync(brandName)
                : product.Brand;

            if (categoryId is not null && await categoryRepository.GetCategoryByIdAsync(categoryId.Value) is null)
                throw new MarketTrackerServiceException(
                    new CategoryFetchingError.CategoryByIdNotFound(categoryId.Value)
                );

            return (await productRepository.UpdateProductAsync(
                productId,
                name,
                imageUrl,
                quantity,
                unit?.GetUnitName(),
                brand.Id,
                categoryId
            ))!;
        });
    }

    public async Task<ProductId> RemoveProductAsync(string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
            (await productRepository.RemoveProductAsync(productId))?.Id ?? throw new MarketTrackerServiceException(
                new ProductFetchingError.ProductByIdNotFound(productId)
            ));
    }
}