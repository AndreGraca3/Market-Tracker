using market_tracker_webapi.Application.Domain.Schemas.List;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List;

public record ShoppingListItemOutputModel(
    string Id,
    string Name,
    DateTime? ArchivedAt,
    DateTime CreatedAt,
    Guid OwnerId,
    bool IsOwner,
    bool IsArchived
);

public static class ListItemOutputModelMapper
{
    public static ShoppingListItemOutputModel ToOutputModel(this ShoppingListItem list, Guid authUserId)
    {
        return new ShoppingListItemOutputModel(
            list.Id,
            list.Name,
            list.ArchivedAt,
            list.CreatedAt,
            list.OwnerId,
            list.OwnerId == authUserId,
            list.IsArchived
        );
    }
}