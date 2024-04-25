using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Price;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public class ProductPriceService(IPriceRepository priceRepository) : IProductPriceService
{
    public async Task<Either<ProductFetchingError, CollectionOutputModel<CompanyPricesOutputModel>>>
        GetProductPricesAsync(string productId)
    {
        int? minPrice = null;
        int? maxPrice = null;

        var storesAvailability = await priceRepository.GetStoresAvailabilityAsync(
            productId
        );

        var companyStoresDictionary = new Dictionary<int, List<StorePriceOutputModel>>();
        var companiesDictionary = new Dictionary<int, CompanyInfo>();

        foreach (var storeAvailability in storesAvailability)
        {
            var storePrice = await priceRepository.GetStorePriceByProductIdAsync(
                productId,
                storeAvailability.StoreId,
                DateTime.Now
            );

            if (!companyStoresDictionary.ContainsKey(storePrice.Store.Company.Id))
            {
                companyStoresDictionary[storePrice.Store.Company.Id] =
                    new List<StorePriceOutputModel>();
            }

            if (minPrice is null || storePrice.PriceData.Price < minPrice)
            {
                minPrice = storePrice.PriceData.Price;
            }

            if (maxPrice is null || storePrice.PriceData.Price > maxPrice)
            {
                maxPrice = storePrice.PriceData.Price;
            }

            companyStoresDictionary[storePrice.Store.Company.Id]
                .Add(
                    StorePriceOutputModel.ToStorePriceOutputModel(
                        Domain.Store.ToStore(storePrice.Store),
                        storePrice.Store.City,
                        storePrice.PriceData.Price,
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