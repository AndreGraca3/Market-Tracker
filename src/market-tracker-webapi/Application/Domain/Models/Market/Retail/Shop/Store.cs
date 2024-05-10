namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

public record Store(int Id, string Name, string Address, City? City, Company Company, Guid OperatorId)
{
    public bool IsOnline => City is null;
}

public record StoreItem(int Id, string Name, string Address, int? CityId, int CompanyId, Guid OperatorId);