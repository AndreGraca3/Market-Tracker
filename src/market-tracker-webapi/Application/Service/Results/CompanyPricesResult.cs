using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;

namespace market_tracker_webapi.Application.Service.Results;

public record CompanyPricesResult(int Id, string Name, List<StoreOffer> Stores);
