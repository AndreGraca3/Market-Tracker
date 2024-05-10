namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

public record Store(StoreId Id, string Name, string Address, City? City, Company Company, Guid OperatorId)
{
    public Store(
        int Id,
        string Name,
        string Address,
        City? City,
        Company Company,
        Guid OperatorId
    ) : this(
        new StoreId(Id),
        Name,
        Address,
        City,
        Company,
        OperatorId
    )
    {
    }


    public bool IsOnline => City is null;
}

public record StoreItem(int Id, string Name, string Address, int? CityId, int CompanyId, Guid OperatorId);

public record StoreId(
    int Value
);