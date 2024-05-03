using market_tracker_webapi.Application.Service.Errors.List;

namespace market_tracker_webapi.Application.Http.Problem;

public class ListClientProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class ClientAlreadyInList(ListClientCreationError.ClientAlreadyInList data)
        : ListClientProblem(
            409,
            "client-already-in-list",
            "Client already in list",
            "Client is already in the list",
            data
        );
    
    public class ClientInListNotFound(ListClientFetchingError.ClientInListNotFound data)
        : ListClientProblem(
            404,
            "client-in-list-not-found",
            "Client in list not found",
            "Client is not in this list",
            data
        );
}