namespace market_tracker_webapi.Application.Services.Errors.Company;

public class CompanyCreationError : ICompanyError
{
    public class CompanyNameAlreadyExists(string name) : CompanyCreationError
    {
        public string Name { get; } = name;
    }
}