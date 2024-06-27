using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Domain.Schemas.List;

public record ShoppingList(
    ShoppingListId Id,
    string Name,
    DateTime? ArchivedAt,
    DateTime CreatedAt,
    UserId OwnerId,
    IEnumerable<UserId> MemberIds)
{
    public ShoppingList(
        string Id,
        string Name,
        DateTime? ArchivedAt,
        DateTime CreatedAt,
        Guid OwnerId,
        IEnumerable<Guid> MemberIds
    ) : this(
        new ShoppingListId(Id),
        Name,
        ArchivedAt,
        CreatedAt,
        new UserId(OwnerId),
        MemberIds.Select(id => new UserId(id))
    )
    {
    }

    public bool IsArchived => ArchivedAt.HasValue;

    public bool IsOwner(Guid userId) => OwnerId.Value.Equals(userId);

    public bool IsMember(Guid userId) => MemberIds.Any(id => id.Value.Equals(userId));

    public bool BelongsTo(Guid userId) => IsOwner(userId) || IsMember(userId);
}

public record ShoppingListItem(
    string Id,
    string Name,
    DateTime? ArchivedAt,
    DateTime CreatedAt,
    Guid OwnerId
)
{
    public bool IsArchived => ArchivedAt.HasValue;
}

public record ShoppingListId(string Value);