using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;

public record ShoppingListEntriesResultOutputModel(
    IEnumerable<ListEntryOfferOutputModel> Entries,
    int TotalPrice,
    int TotalProducts
);

public static class ShoppingListEntriesOutputModelMapper
{
    public static ShoppingListEntriesResultOutputModel ToOutputModel(this ShoppingListEntriesResult shoppingListEntriesResult)
    {
        return new ShoppingListEntriesResultOutputModel(
            shoppingListEntriesResult.Entries.Select(entryOffer => entryOffer.ToOutputModel()),
            shoppingListEntriesResult.TotalPrice,
            shoppingListEntriesResult.TotalProducts
        );
    }
}