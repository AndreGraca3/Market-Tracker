namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;

public record CompanyPricesOutputModel(int Id, string Name, List<StoreOfferOutputModel> Stores);
