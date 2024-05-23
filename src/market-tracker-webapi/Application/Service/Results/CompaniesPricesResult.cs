using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;

namespace market_tracker_webapi.Application.Service.Results;

public record CompaniesPricesResult(
    IEnumerable<CompanyPrices> Companies,
    int MinPrice,
    int MaxPrice
);

public record CompanyPrices(
    int Id,
    string Name,
    IEnumerable<StoreOffer> Stores
);