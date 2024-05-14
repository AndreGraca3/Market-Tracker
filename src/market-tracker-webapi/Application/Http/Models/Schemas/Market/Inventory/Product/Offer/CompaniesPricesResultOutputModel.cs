using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;
using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Offer;

public record CompaniesPricesResultOutputModel(
    IEnumerable<CompanyPricesOutputModel> Companies,
    int MinPrice,
    int MaxPrice
);

public record CompanyPricesOutputModel(
    int Id,
    string Name,
    IEnumerable<StoreOfferResultOutputModel> Stores
);

public static class CompaniesPricesOutputModelMapper
{
    public static CompaniesPricesResultOutputModel ToOutputModel(this CompaniesPricesResult companiesPricesResult)
    {
        return new CompaniesPricesResultOutputModel(
            companiesPricesResult.Companies.Select(companyPricesResult => companyPricesResult.ToOutputModel()),
            companiesPricesResult.MinPrice,
            companiesPricesResult.MaxPrice
        );
    }

    public static CompanyPricesOutputModel ToOutputModel(this CompanyPrices companyPrices)
    {
        return new CompanyPricesOutputModel(
            companyPrices.Id,
            companyPrices.Name,
            companyPrices.Stores.Select(storeOfferResult => storeOfferResult.ToOutputModel())
        );
    }
}