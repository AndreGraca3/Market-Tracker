using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

using Company = Domain.Models.Market.Retail.Shop.Company;

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

        var companyStoresDictionary = new Dictionary<int, List<StoreOfferOutputModel>>();
        var companiesDictionary = new Dictionary<int, Company>();

        foreach (var storeAvailability in storesAvailability)
        {
            var storeOffer = await priceRepository.GetStoreOfferAsync(
                productId,
                storeAvailability.StoreId,
                DateTime.Now
            );

            if (!companyStoresDictionary.ContainsKey(storeOffer!.Store.Company.Id))
            {
                companyStoresDictionary[storeOffer.Store.Company.Id] =
                    new List<StoreOfferOutputModel>();
            }

            if (minPrice is null || storeOffer.PriceData.FinalPrice < minPrice)
            {
                minPrice = storeOffer.PriceData.FinalPrice;
            }

            if (maxPrice is null || storeOffer.PriceData.FinalPrice > maxPrice)
            {
                maxPrice = storeOffer.PriceData.FinalPrice;
            }

            companyStoresDictionary[storeOffer.Store.Company.Id]
                .Add(
                    StoreOfferOutputModel.ToStoreOfferOutputModel(
                        StoreOutputModel.ToStoreOutputModel(storeOffer.Store),
                        storeOffer.Store.City,
                        storeOffer.PriceData,
                        storeAvailability.IsAvailable,
                        storeAvailability.LastChecked
                    )
                );
            companiesDictionary[storeOffer.Store.Company.Id] = storeOffer.Store.Company;
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