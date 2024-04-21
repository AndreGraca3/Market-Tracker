namespace market_tracker_webapi.Application.Service.Errors.ListEntry;

public class ListEntryFetchingError : IListEntryError
{
    public class ListEntryByIdNotFound(int listId, string productId) : ListEntryFetchingError
    {
        public int ListId { get; } = listId;
        public string ProductId { get; } = productId;
    }
}