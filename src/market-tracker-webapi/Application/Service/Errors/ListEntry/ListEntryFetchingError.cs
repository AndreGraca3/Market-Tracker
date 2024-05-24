namespace market_tracker_webapi.Application.Service.Errors.ListEntry;

public class ListEntryFetchingError : IListEntryError
{
    public class ListEntryByIdNotFound(string entryId) : ListEntryFetchingError
    {
        public string EntryId { get; } = entryId;
    }
}