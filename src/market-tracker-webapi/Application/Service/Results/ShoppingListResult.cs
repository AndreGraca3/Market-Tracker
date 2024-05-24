using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Service.Results;

public record ShoppingListResult(
    string Id,
    string Name,
    DateTime? ArchivedAt,
    DateTime CreatedAt,
    ClientItem Owner,
    IEnumerable<ClientItem> Members
);