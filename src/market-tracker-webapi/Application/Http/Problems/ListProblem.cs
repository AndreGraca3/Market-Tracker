using market_tracker_webapi.Application.Service.Errors.List;

namespace market_tracker_webapi.Application.Http.Problems;

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
            $"List with id {data.ListId} not found",
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

    public class UserDoesNotOwnList(ListFetchingError.ClientDoesNotOwnList data) : ListProblem(
        403,
        "user-does-not-own-list",
        "User does not own list",
        "You do not own this list",
        data
    );
    
    public class MaxListNumberReached(ListCreationError.MaxListNumberReached data) : ListProblem(
        403,
        "max-list-number-reached",
        "Max list number reached",
        $"Max list number reached for client {data.ClientId}",
        data
    );
    
    public class ClientAlreadyInList(ListCreationError.ClientAlreadyInList data)
        : ListProblem(
            409,
            "client-already-in-list",
            "Client already in list",
            "Client is already in the list",
            data
        );

    public class ClientDoesNotBelongToList(ListFetchingError.ClientDoesNotBelongToList data)
        : ListProblem(
            404,
            "client-does-not-belong-to-list",
            "Client does not belong to list",
            "You do not belong to this list",
            data
        );
    
    public static ListProblem FromServiceError(IListError error)
    {
        return error switch
        {
            ListFetchingError.ListByIdNotFound listByIdNotFound => new ListByIdNotFound(listByIdNotFound),
            ListCreationError.ListNameAlreadyExists listNameAlreadyExists => new ListNameAlreadyExists(listNameAlreadyExists),
            ListUpdateError.ListIsArchived listIsArchived => new ListIsArchived(listIsArchived),
            ListFetchingError.ClientDoesNotOwnList userDoesNotOwnList => new UserDoesNotOwnList(userDoesNotOwnList),
            ListCreationError.MaxListNumberReached maxListNumberReached => new MaxListNumberReached(maxListNumberReached),
            ListCreationError.ClientAlreadyInList clientAlreadyInList => new ClientAlreadyInList(clientAlreadyInList),
            ListFetchingError.ClientDoesNotBelongToList clientInListNotFound => new ClientDoesNotBelongToList(clientInListNotFound),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}