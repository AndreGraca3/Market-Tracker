namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListClientFetchingError : IListError
{
    public class ClientInListNotFound(int listId, Guid clientId) : ListClientFetchingError
    {
        public int ListId { get; } = listId;
        public Guid ClientId { get; } = clientId;
    }
}