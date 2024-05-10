namespace market_tracker_webapi.Application.Domain.Models.List;

public record ShoppingList(ShoppingListId Id, string Name, DateTime? ArchivedAt, DateTime CreatedAt, Guid OwnerId)
{
    public ShoppingList(
        int Id,
        string Name,
        DateTime? ArchivedAt,
        DateTime CreatedAt,
        Guid OwnerId
    ) : this(
        new ShoppingListId(Id),
        Name,
        ArchivedAt,
        CreatedAt,
        OwnerId
    )
    {
    }
};

public record ShoppingListId(int Value);