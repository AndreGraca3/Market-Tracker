namespace market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

public record Company(CompanyId Id, string Name, DateTime CreatedAt)
{
    public Company(
        int Id,
        string Name,
        DateTime CreatedAt
    ) : this(
        new CompanyId(Id),
        Name,
        CreatedAt
    )
    {
    }
};

public record CompanyId(
    int Value
);