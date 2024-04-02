namespace market_tracker_webapi.Application.Service.Errors.Company;

public class CompanyFetchingError : ICompanyError
{
    public class CompanyByIdNotFound(int id) : CompanyFetchingError
    {
        public int Id { get; } = id;
    }

    public class CompanyByNameNotFound(string companyName) : CompanyFetchingError
    {
        public string Name { get; } = companyName;
    }
}
