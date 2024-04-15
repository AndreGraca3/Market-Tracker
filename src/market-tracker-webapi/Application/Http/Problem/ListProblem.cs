using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.User;

namespace market_tracker_webapi.Application.Http.Problem;

public class ListProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class ListByIdNotFound(ListFetchingError.ListByIdNotFound data)
        : ListProblem(
            404,
            "list-not-found",
            "List not found",
            $"List with id {data.Id} not found",
            data
        );

    public class ListNameAlreadyExists(ListCreationError.ListNameAlreadyExists data)
        : ListProblem(
            409,
            "list-name-already-exists",
            "List name already exists",
            $"List name {data.ListName} already exists for client {data.ClientId}",
            data
        );

    public class ListIsArchived(ListUpdateError.ListIsArchived data)
        : ListProblem(
            409,
            "list-is-archived",
            "List is archived",
            "List is already archived",
            data
        );

    public class UserDoesNotOwnList(ListFetchingError.UserDoesNotOwnList data) : ListProblem(
        403,
        "user-does-not-own-list",
        "User does not own list",
        "You do not own this list",
        data
    );
}