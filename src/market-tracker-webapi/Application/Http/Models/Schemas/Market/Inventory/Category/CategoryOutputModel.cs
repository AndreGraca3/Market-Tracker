namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Category;

using Category = Domain.Schemas.Market.Inventory.Category;

public record CategoryOutputModel(
    int Id,
    string Name
);

public static class CategoryOutputModelMapper
{
    public static CategoryOutputModel ToOutputModel(this Category category)
    {
        return new CategoryOutputModel(category.Id.Value, category.Name);
    }
}