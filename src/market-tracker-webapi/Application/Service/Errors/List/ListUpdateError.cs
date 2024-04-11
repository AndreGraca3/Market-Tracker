namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListUpdateError : IListError
{
    public class ListIsArchived(int listId) : ListUpdateError
    {
        public int ListId { get; } = listId;
    }

}
    
