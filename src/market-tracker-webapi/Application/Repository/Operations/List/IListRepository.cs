using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public interface IListRepository
{
    Task<IEnumerable<ListOfProducts>> GetListsAsync(Guid clientId, string? listName = null,
        DateTime? createdAfter = null, bool? isArchived = null
    );

    Task<ListOfProducts?> GetListByIdAsync(int id);

    Task<int> AddListAsync(Guid clientId, string listName);

    Task<ListOfProducts?> UpdateListAsync(int id, string? listName = null, DateTime? archivedAt = null);

    Task<ListOfProducts?> DeleteListAsync(int id);
}