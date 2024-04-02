using market_tracker_webapi.Application.Service.Errors.City;

namespace market_tracker_webapi.Application.Http.Problem;

public class CityProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class CityByIdNotFound(CityFetchingError.CityByIdNotFound data)
        : CompanyProblem(
            404,
            "city-not-found",
            "City not found",
            $"City with id {data.Id} not found",
            data
        );

    public class CityNameAlreadyExists(CityCreationError.CityNameAlreadyExists data)
        : CompanyProblem(
            409,
            "city-name-already-exists",
            "City name already exists",
            "A city with that name already exists",
            data
        );
}