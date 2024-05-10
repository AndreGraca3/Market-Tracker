namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory;

public record Category(CategoryId Id, string Name)
{
    public Category(
        int Id,
        string Name
    ) : this(
        new CategoryId(Id),
        Name
    )
    {
    }
};

public record CategoryId(
    int Value
);