using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.List;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Results;
using market_tracker_webapi.Application.Service.Transaction;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.List;

public class ListService(
    IListRepository listRepository,
    IClientRepository clientRepository,
    ITransactionManager transactionManager) : IListService
{
    public static readonly int MaxListNumber = 10;

    public async Task<IEnumerable<ShoppingList>> GetListsAsync(Guid clientId,
        bool? isOwner, string? listName, DateTime? createdAfter, bool? isArchived)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var lists
                = await listRepository.GetListsFromClientAsync(clientId, isOwner, listName, createdAfter, isArchived);
            return lists;
        });
    }

    public async Task<ShoppingListResult> GetListByIdAsync(string listId, Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                throw new MarketTrackerServiceException(new ListFetchingError.ListByIdNotFound(listId));

            var listClients = (await listRepository.GetClientMembersByListIdAsync(listId)).ToList();
            if (!list.BelongsTo(clientId))
                throw new MarketTrackerServiceException(
                    new ListFetchingError.ClientDoesNotBelongToList(clientId, listId));

            var listOwner = await clientRepository.GetClientByIdAsync(list.OwnerId.Value) ??
                            throw new MarketTrackerServiceException(
                                new UserFetchingError.UserByIdNotFound(list.OwnerId.Value));

            return new ShoppingListResult(
                list.Id.Value,
                list.Name,
                list.ArchivedAt,
                list.CreatedAt,
                listOwner.ToClientItem(),
                listClients
            );
        });
    }

    public async Task<ShoppingListId> AddListAsync(Guid clientId, string listName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (!(await listRepository.GetListsFromClientAsync(clientId, true, listName)).IsNullOrEmpty())
                throw new MarketTrackerServiceException(
                    new ListCreationError.ListNameAlreadyExists(clientId, listName));

            if ((await listRepository.GetListsFromClientAsync(clientId, true)).Count() >= MaxListNumber)
                throw new MarketTrackerServiceException(
                    new ListCreationError.MaxListNumberReached(clientId, MaxListNumber));

            var listId = await listRepository.AddListAsync(listName, clientId);
            await listRepository.AddListMemberAsync(listId.Value, clientId);
            return listId;
        });
    }

    public async Task<ShoppingListItem> UpdateListAsync(string listId, Guid clientId, string? listName,
        bool? isArchived)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                throw new MarketTrackerServiceException(new ListFetchingError.ListByIdNotFound(listId));

            if (!list.IsOwner(clientId))
                throw new MarketTrackerServiceException(new ListFetchingError.ClientDoesNotOwnList(clientId, listId));

            if (list.ArchivedAt != null)
                throw new MarketTrackerServiceException(new ListUpdateError.ListIsArchived(listId));

            if (listName is not null &&
                !(await listRepository.GetListsFromClientAsync(clientId, true, listName)).IsNullOrEmpty())
                throw new MarketTrackerServiceException(
                    new ListCreationError.ListNameAlreadyExists(clientId, listName));

            DateTime? archivedAt = isArchived.HasValue && isArchived.Value ? DateTime.UtcNow : null;

            return (await listRepository.UpdateListAsync(listId, archivedAt, listName))!;
        });
    }

    public async Task<ShoppingListItem> DeleteListAsync(string listId, Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                throw new MarketTrackerServiceException(new ListFetchingError.ListByIdNotFound(listId));

            if (!list.IsOwner(clientId))
                throw new MarketTrackerServiceException(new ListFetchingError.ClientDoesNotOwnList(clientId, listId));

            return (await listRepository.DeleteListAsync(listId))!;
        });
    }

    public async Task<ListClient> AddClientToListAsync(string listId,
        Guid clientId, Guid clientIdToAdd)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await clientRepository.GetClientByIdAsync(clientIdToAdd) is null)
                throw new MarketTrackerServiceException(new UserFetchingError.UserByIdNotFound(clientIdToAdd));

            var list = await listRepository.GetListByIdAsync(listId) ??
                       throw new MarketTrackerServiceException(new ListFetchingError.ListByIdNotFound(listId));

            if (!list.IsOwner(clientId))
                throw new MarketTrackerServiceException(new ListFetchingError.ClientDoesNotOwnList(clientId, listId));

            if (list.BelongsTo(clientIdToAdd))
                throw new MarketTrackerServiceException(
                    new ListCreationError.ClientAlreadyInList(listId, clientIdToAdd));

            await listRepository.AddListMemberAsync(listId, clientIdToAdd);
            return new ListClient(clientIdToAdd, listId);
        });
    }

    public async Task<ListClient> RemoveClientFromListAsync(string listId, Guid clientId,
        Guid clientIdToRemove)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId) ??
                       throw new MarketTrackerServiceException(new ListFetchingError.ListByIdNotFound(listId));

            if (!list.IsOwner(clientId))
                throw new MarketTrackerServiceException(new ListFetchingError.ClientDoesNotOwnList(clientId, listId));

            if (!list.IsMember(clientIdToRemove))
                throw new MarketTrackerServiceException(
                    new ListFetchingError.ClientDoesNotBelongToList(clientIdToRemove, listId));

            await listRepository.DeleteListMemberAsync(listId, clientIdToRemove);
            return new ListClient(clientIdToRemove, listId);
        });
    }
}