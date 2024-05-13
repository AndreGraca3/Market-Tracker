using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Price;
using market_tracker_webapi.Application.Http.Models.Store;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Operations.Price;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public class ProductPriceService(IProductRepository productRepository, IPriceRepository priceRepository)
    : IProductPriceService
{
    public async Task<Either<ProductFetchingError, CollectionOutputModel<CompanyPricesOutputModel>>>
        GetProductPricesAsync(string productId)
    {
        if (await productRepository.GetProductByIdAsync(productId) is null)
        {
            return EitherExtensions.Failure<ProductFetchingError, CollectionOutputModel<CompanyPricesOutputModel>>(
                new ProductFetchingError.ProductByIdNotFound(productId)
            );
        }

        int? minPrice = null;
        int? maxPrice = null;

        var storesAvailability = await priceRepository.GetStoresAvailabilityAsync(
            productId
        );

        var companyStoresDictionary = new Dictionary<int, List<StorePriceOutputModel>>();
        var companiesDictionary = new Dictionary<int, CompanyInfo>();

        foreach (var storeAvailability in storesAvailability)
        {
            var storePrice = await priceRepository.GetStorePriceAsync(
                productId,
                storeAvailability.StoreId,
                DateTime.Now
            );

            if (!companyStoresDictionary.ContainsKey(storePrice!.Store.Company.Id))
            {
                companyStoresDictionary[storePrice.Store.Company.Id] =
                    new List<StorePriceOutputModel>();
            }

            if (minPrice is null || storePrice.PriceData.FinalPrice < minPrice)
            {
                minPrice = storePrice.PriceData.FinalPrice;
            }

            if (maxPrice is null || storePrice.PriceData.FinalPrice > maxPrice)
            {
                maxPrice = storePrice.PriceData.FinalPrice;
            }

            companyStoresDictionary[storePrice.Store.Company.Id]
                .Add(
                    StorePriceOutputModel.ToStorePriceOutputModel(
                        StoreOutputModel.ToStoreOutputModel(storePrice.Store),
                        storePrice.Store.City,
                        storePrice.PriceData.FinalPrice,
                        storePrice.PriceData.Promotion,
                        storeAvailability.IsAvailable,
                        storeAvailability.LastChecked
                    )
                );
            companiesDictionary[storePrice.Store.Company.Id] = storePrice.Store.Company;
        }

        return EitherExtensions.Success<ProductFetchingError, CollectionOutputModel<CompanyPricesOutputModel>>(
            new CollectionOutputModel<CompanyPricesOutputModel>(
                companyStoresDictionary
                    .Select(companyStores => new CompanyPricesOutputModel(
                        companiesDictionary[companyStores.Key].Id,
                        companiesDictionary[companyStores.Key].Name,
                        companyStores.Value
                    ))
                    .ToList()
            )
        );
    }
}