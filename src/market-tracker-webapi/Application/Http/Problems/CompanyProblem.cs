using market_tracker_webapi.Application.Service.Errors.Company;

namespace market_tracker_webapi.Application.Http.Problems;

public class CompanyProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problems.Problem(status, subType, title, detail, data)
{
    public class CompanyByIdNotFound(CompanyFetchingError.CompanyByIdNotFound data) : CompanyProblem(
        404,
        "company-not-found",
        "Company not found",
        $"Company with id {data.Id} not found",
        data
    );

    public class CompanyNameAlreadyExists(CompanyCreationError.CompanyNameAlreadyExists data) : CompanyProblem(
        409,
        "company-name-already-exists",
        "Company name already exists",
        "A company with that name already exists",
        data
    );
    
    public static CompanyProblem FromServiceError(ICompanyError error)
    {
        return error switch
        {
            CompanyFetchingError.CompanyByIdNotFound companyByIdNotFound => new CompanyByIdNotFound(companyByIdNotFound),
            CompanyCreationError.CompanyNameAlreadyExists companyNameAlreadyExists => new CompanyNameAlreadyExists(companyNameAlreadyExists),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}