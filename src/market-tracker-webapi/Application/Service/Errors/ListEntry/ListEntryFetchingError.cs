namespace market_tracker_webapi.Application.Service.Errors.ListEntry;

public class ListEntryFetchingError : IListEntryError
{
    public class ListEntryByIdNotFound(string listId, string productId) : ListEntryFetchingError
    {
        public string ListId { get; } = listId;
        public string ProductId { get; } = productId;
    }
}