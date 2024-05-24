using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

using Company = Domain.Schemas.Market.Retail.Shop.Company;

public class ProductPriceService(IPriceRepository priceRepository) : IProductPriceService
{
    public async Task<CompaniesPricesResult> GetProductPricesAsync(string productId)
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

            if (storeOffer is null)
            {
                // never happens because we only loop stores that have availability registered
                // (not necessarily available)
                continue;
            }

            if (!companyStoresDictionary.ContainsKey(storeOffer.Store.Company.Id.Value))
            {
                companyStoresDictionary[storeOffer.Store.Company.Id.Value] = [];
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

        var companiesPrices = companyStoresDictionary
            .Select(companyStores =>
            {
                var company = companiesDictionary[companyStores.Key];
                return new CompanyPrices(
                    company.Id.Value,
                    company.Name,
                    company.LogoUrl,
                    companyStores.Value
                        .OrderBy(storeOffer => !storeOffer.StoreAvailability.IsAvailable)
                        .ToList()
                );
            });

        return new CompaniesPricesResult(
            companiesPrices.ToList(),
            minPrice ?? 0,
            maxPrice ?? 0
        );
    }
}