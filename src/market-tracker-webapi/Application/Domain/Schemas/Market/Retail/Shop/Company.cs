namespace market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

public record Company(CompanyId Id, string Name, string LogoUrl, DateTime CreatedAt)
{
    public Company(
        int Id,
        string Name,
        string LogoUrl,
        DateTime CreatedAt
    ) : this(
        new CompanyId(Id),
        Name,
        LogoUrl,
        CreatedAt
    )
    {
    }
};

public record CompanyId(
    int Value
);