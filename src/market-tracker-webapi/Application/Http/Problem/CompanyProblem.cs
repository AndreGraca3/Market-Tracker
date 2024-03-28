using market_tracker_webapi.Application.Services.Errors.Company;

namespace market_tracker_webapi.Application.Http.Problem;

public class CompanyProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class CompanyByIdNotFound(CompanyFetchingError.CompanyByIdNotFound data)
        : CompanyProblem(
            404,
            "company-not-found",
            "Company not found",
            $"Company with id {data.Id} not found",
            data
        );

    public class CompanyNameAlreadyExists()
        : CompanyProblem(
            409,
            "company-name-already-exists",
            "Company name already exists",
            "A company with that name already exists"
        );
}