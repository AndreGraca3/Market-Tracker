namespace market_tracker_webapi.Application.Service.Errors.Company;

public class CompanyCreationError : ICompanyError
{
    public class CompanyNameAlreadyExists(string companyName) : CompanyCreationError
    {
        public string Name { get; } = companyName;
    }
}
