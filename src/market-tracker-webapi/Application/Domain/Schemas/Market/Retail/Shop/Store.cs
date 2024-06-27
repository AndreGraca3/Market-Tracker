namespace market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

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

    public StoreItem ToStoreItem() =>
        new StoreItem(Id.Value, Name, Address, City?.Id.Value, Company.Id.Value, OperatorId);
}

public record StoreItem(StoreId Id, string Name, string Address, int? CityId, int CompanyId, Guid OperatorId)
{
    public StoreItem(
        int Id,
        string Name,
        string Address,
        int? CityId,
        int CompanyId,
        Guid OperatorId
    ) : this(
        new StoreId(Id),
        Name,
        Address,
        CityId,
        CompanyId,
        OperatorId
    )
    {
    }
}

public record StoreId(
    int Value
);