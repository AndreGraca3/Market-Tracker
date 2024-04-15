namespace market_tracker_webapi.Application.Service.Errors.ListEntry;

public class ListEntryCreationError : IListEntryError
{
    public class ListEntryQuantityInvalid(int? quantity) : ListEntryCreationError
    {
        public int? Quantity { get; } = quantity;
    }
    
    public class ProductAlreadyInList(int listId, string productId) : ListEntryCreationError
    {
        public int ListId { get; } = listId;
        public string ProductId { get; } = productId;
    }
}