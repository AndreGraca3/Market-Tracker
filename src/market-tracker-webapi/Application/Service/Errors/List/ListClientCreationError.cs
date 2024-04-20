namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListClientCreationError : IServiceError
{
    public class ClientAlreadyInList(int listId, Guid clientId) : ListClientCreationError
    {
        public int ListId { get; } = listId;
        public Guid ClientId { get; } = clientId;
    }

    public class ClientInListNotFound(int listId, Guid clientId) : ListClientCreationError
    {
        public int ListId { get; } = listId;
        public Guid ClientId { get; } = clientId;
    }
}