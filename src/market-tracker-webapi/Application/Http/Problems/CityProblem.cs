using market_tracker_webapi.Application.Service.Errors.City;

namespace market_tracker_webapi.Application.Http.Problems;

public class CityProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class CityByIdNotFound(CityFetchingError.CityByIdNotFound data) : CityProblem(
        404,
        "city-not-found",
        "City not found",
        $"City with id {data.Id} not found",
        data
    );

    public class CityNameAlreadyExists(CityCreationError.CityNameAlreadyExists data) : CityProblem(
        409,
        "city-name-already-exists",
        "City name already exists",
        "A city with that name already exists",
        data
    );

    public class CityNameNotFound(CityFetchingError.CityByNameNotFound data) : CityProblem(
        404,
        "city-name-not-found",
        "City name not found",
        $"City with name {data.Name} not found",
        data
    );

    public static CityProblem FromServiceError(ICityError error)
    {
        return error switch
        {
            CityFetchingError.CityByIdNotFound cityByIdNotFound => new CityByIdNotFound(cityByIdNotFound),
            CityCreationError.CityNameAlreadyExists cityNameAlreadyExists => new CityNameAlreadyExists(
                cityNameAlreadyExists),
            CityFetchingError.CityByNameNotFound cityByNameNotFound => new CityNameNotFound(cityByNameNotFound),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}