using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Dto;

public record CompanyInfo(int Id, string Name)
{
    public static CompanyInfo ToCompanyInfo(Company company)
    {
        return new CompanyInfo(company.Id, company.Name);
    }
}