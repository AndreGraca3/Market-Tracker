using market_tracker_webapi.Application.Service.Errors.ListEntry;

namespace market_tracker_webapi.Application.Http.Problem;

public class ListEntryProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class ListEntryByIdNotFound(ListEntryFetchingError.ListEntryByIdNotFound data)
        : ListEntryProblem(
            404,
            "list-entry-not-found",
            "List entry not found",
            $"This product is not present in given list",
            data
        );
    
    public class ListEntryQuantityInvalid(ListEntryCreationError.ListEntryQuantityInvalid data)
        : ListEntryProblem(
            400,
            "list-entry-quantity-invalid",
            "List entry quantity invalid",
            $"Invalid quantity",
            data
        );
}