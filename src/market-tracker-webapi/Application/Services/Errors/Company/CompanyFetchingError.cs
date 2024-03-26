namespace market_tracker_webapi.Application.Services.Errors.Company;

public class CompanyFetchingError : ICompanyError
{
    public class CompanyByIdNotFound(int id) : CompanyFetchingError
    {
        public int Id { get; } = id;
    }
    
    public class CompanyByNameNotFound(string name) : CompanyFetchingError
    {
        public string Name { get; } = name;
    }
}