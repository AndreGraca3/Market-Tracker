namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListUpdateError : IListError
{
    public class ListIsArchived(string listId) : ListUpdateError
    {
        public string ListId { get; } = listId;
    }

}
    
