using market_tracker_webapi.Application.Domain.Schemas.List;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List;

public record ShoppingListOutputModel(
    string Id,
    string Name,
    DateTime? ArchivedAt,
    DateTime CreatedAt,
    Guid OwnerId,
    bool IsOwner,
    bool IsArchived
);

public static class ListOutputModelMapper
{
    public static ShoppingListOutputModel ToOutputModel(this ShoppingList list, Guid authUserId)
    {
        return new ShoppingListOutputModel(
            list.Id.Value,
            list.Name,
            list.ArchivedAt,
            list.CreatedAt,
            list.OwnerId.Value,
            list.OwnerId.Value == authUserId,
            list.IsArchived
        );
    }
    
    public static ShoppingListOutputModel ToOutputModel(this ShoppingListItem list, Guid authUserId)
    {
        return new ShoppingListOutputModel(
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