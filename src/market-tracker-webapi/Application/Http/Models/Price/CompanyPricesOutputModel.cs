namespace market_tracker_webapi.Application.Http.Models.Price;

public record CompanyPricesOutputModel(int Id, string Name, List<StorePriceOutputModel> Stores);
