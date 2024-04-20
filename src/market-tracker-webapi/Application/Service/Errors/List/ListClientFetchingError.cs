namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListClientFetchingError : IServiceError
{
    public class ClientInListNotFound(Guid clientId, int listId) : ListClientFetchingError
    {
        public Guid ClientId { get; } = clientId;
        public int ListId { get; } = listId;
    }
}