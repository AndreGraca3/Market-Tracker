namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListCreationError : IServiceError
{
    public class ListNameAlreadyExists(Guid clientId, string listName) : ListCreationError
    {
        public Guid ClientId { get; } = clientId;
        public string ListName { get; } = listName;
    }
}