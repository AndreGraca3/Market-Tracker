namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Company;

using Company = Domain.Schemas.Market.Retail.Shop.Company;

public record CompanyOutputModel(
    int Id,
    string Name,
    string LogoUrl,
    DateTime CreatedAt
);

public static class CompanyOutputModelMapper
{
    public static CompanyOutputModel ToOutputModel(this Company company)
    {
        return new CompanyOutputModel(
            company.Id.Value,
            company.Name,
            company.LogoUrl,
            company.CreatedAt
        );
    }
}