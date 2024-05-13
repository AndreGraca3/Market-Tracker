namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListCreationError : IListError
{
    public class ListNameAlreadyExists(Guid clientId, string listName) : ListCreationError
    {
        public Guid ClientId { get; } = clientId;
        public string ListName { get; } = listName;
    }

    public class MaxListNumberReached(Guid clientId, int maxListCounter) : ListCreationError
    {
        public Guid ClientId { get; } = clientId;
        public int MaxListCounter { get; } = maxListCounter;
    }

    public class ClientAlreadyInList(string listId, Guid clientId) : ListCreationError
    {
        public string ListId { get; } = listId;
        public Guid ClientId { get; } = clientId;
    }
}