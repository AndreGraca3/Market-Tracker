using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Results;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

using Company = Domain.Models.Market.Retail.Shop.Company;

public class ProductPriceService(IPriceRepository priceRepository) : IProductPriceService
{
    public async Task<Either<ProductFetchingError, IEnumerable<CompanyPricesResult>>> GetProductPricesAsync(
        string productId)
    {
        int? minPrice = null;
        int? maxPrice = null;

        var storesAvailability = await priceRepository.GetStoresAvailabilityAsync(
            productId
        );

        var companyStoresDictionary = new Dictionary<int, List<StoreOffer>>();
        var companiesDictionary = new Dictionary<int, Company>();

        foreach (var storeAvailability in storesAvailability)
        {
            var storeOffer = await priceRepository.GetStoreOfferAsync(
                productId,
                storeAvailability.StoreId,
                DateTime.Now
            );

            if (!companyStoresDictionary.ContainsKey(storeOffer!.Store.Company.Id.Value))
            {
                companyStoresDictionary[storeOffer.Store.Company.Id.Value] =
                    new List<StoreOffer>();
            }

            if (minPrice is null || storeOffer.PriceData.FinalPrice < minPrice)
            {
                minPrice = storeOffer.PriceData.FinalPrice;
            }

            if (maxPrice is null || storeOffer.PriceData.FinalPrice > maxPrice)
            {
                maxPrice = storeOffer.PriceData.FinalPrice;
            }

            companyStoresDictionary[storeOffer.Store.Company.Id.Value]
                .Add(storeOffer);
            companiesDictionary[storeOffer.Store.Company.Id.Value] = storeOffer.Store.Company;
        }

        return EitherExtensions.Success<ProductFetchingError, IEnumerable<CompanyPricesResult>>(
            companyStoresDictionary
                .Select(companyStores => new CompanyPricesResult(
                    companiesDictionary[companyStores.Key].Id.Value,
                    companiesDictionary[companyStores.Key].Name,
                    companyStores.Value
                ))
                .ToList()
        );
    }
}