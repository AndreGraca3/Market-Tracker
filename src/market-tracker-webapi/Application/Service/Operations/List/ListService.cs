using market_tracker_webapi.Application.Domain.Models.List;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.List;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Repository.List;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.List;

public class ListService(
    IListRepository listRepository,
    IUserRepository userRepository,
    ITransactionManager transactionManager) : IListService
{
    private const int MaxListNumber = 10;

    public async Task<Either<IServiceError, CollectionOutputModel<ShoppingList>>> GetListsAsync(Guid clientId,
        bool isOwner, string? listName, DateTime? createdAfter, bool? isArchived)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var lists
                = await listRepository.GetListsAsync(clientId, isOwner, listName, createdAfter, isArchived);
            return EitherExtensions.Success<IServiceError, CollectionOutputModel<ShoppingList>>(
                new CollectionOutputModel<ShoppingList>(lists));
        });
    }

    public async Task<Either<ListFetchingError, ShoppingListOutputModel>> GetListByIdAsync(int id, Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ShoppingListOutputModel>(
                    new ListFetchingError.ListByIdNotFound(id));

            var listClients = (await listRepository.GetClientIdsByListIdAsync(id)).ToList();
            if (!listClients.Contains(clientId))
                return EitherExtensions.Failure<ListFetchingError, ShoppingListOutputModel>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, id)
                );

            return EitherExtensions.Success<ListFetchingError, ShoppingListOutputModel>(
                new ShoppingListOutputModel
                {
                    Id = list.Id,
                    Name = list.Name,
                    ArchivedAt = list.ArchivedAt,
                    CreatedAt = list.CreatedAt,
                    ClientIds = listClients
                }
            );
        });
    }

    public async Task<Either<IServiceError, IntIdOutputModel>> AddListAsync(Guid clientId, string listName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (!(await listRepository.GetListsAsync(clientId, true, listName)).IsNullOrEmpty())
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListCreationError.ListNameAlreadyExists(clientId, listName));

            if ((await listRepository.GetListsAsync(clientId, true)).Count() >= MaxListNumber)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListCreationError.MaxListNumberReached(clientId, MaxListNumber));

            var id = await listRepository.AddListAsync(listName, clientId);
            await listRepository.AddListClientAsync(id, clientId);
            return EitherExtensions.Success<IServiceError, IntIdOutputModel>(new IntIdOutputModel(id));
        });
    }

    public async Task<Either<IServiceError, ShoppingList>> UpdateListAsync(int id, Guid clientId, string? listName,
        bool? isArchived
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ShoppingList>(
                    new ListFetchingError.ListByIdNotFound(id));

            if (list.OwnerId != clientId)
                return EitherExtensions.Failure<IServiceError, ShoppingList>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, id));

            if (list.ArchivedAt != null)
                return EitherExtensions.Failure<IServiceError, ShoppingList>(
                    new ListUpdateError.ListIsArchived(id)
                );

            if (listName is not null && !(await listRepository.GetListsAsync(clientId, true, listName)).IsNullOrEmpty())
                return EitherExtensions.Failure<IServiceError, ShoppingList>(
                    new ListCreationError.ListNameAlreadyExists(clientId, listName)
                );

            DateTime? archivedAt = isArchived.HasValue && isArchived.Value ? DateTime.Now : null;

            var updatedList = await listRepository.UpdateListAsync(id, archivedAt, listName);
            return EitherExtensions.Success<IServiceError, ShoppingList>(updatedList!);
        });
    }

    public async Task<Either<ListFetchingError, ShoppingList>> DeleteListAsync(int id, Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ShoppingList>(
                    new ListFetchingError.ListByIdNotFound(id));

            if (list.OwnerId != clientId)
                return EitherExtensions.Failure<ListFetchingError, ShoppingList>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, id));

            var deletedList = await listRepository.DeleteListAsync(id);
            return EitherExtensions.Success<ListFetchingError, ShoppingList>(deletedList!);
        });
    }

    public async Task<Either<IServiceError, ListClient>> AddClientToListAsync(int listId,
        Guid clientId, Guid clientIdToAdd)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByIdAsync(clientIdToAdd) is null)
                return EitherExtensions.Failure<IServiceError, ListClient>(
                    new UserFetchingError.UserByIdNotFound(clientId));

            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListClient>(
                    new ListFetchingError.ListByIdNotFound(listId));

            if (list.OwnerId != clientId)
                return EitherExtensions.Failure<IServiceError, ListClient>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            if (await listRepository.IsClientInListAsync(listId, clientIdToAdd))
                return EitherExtensions.Failure<IServiceError, ListClient>(
                    new ListClientCreationError.ClientAlreadyInList(listId, clientIdToAdd));

            await listRepository.AddListClientAsync(listId, clientIdToAdd);
            return EitherExtensions.Success<IServiceError, ListClient>(new ListClient(
                clientIdToAdd,
                listId
            ));
        });
    }

    public async Task<Either<IListError, ListClient>> RemoveClientFromListAsync(int listId, Guid clientId,
        Guid clientIdToRemove)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                return EitherExtensions.Failure<IListError, ListClient>(
                    new ListFetchingError.ListByIdNotFound(listId));

            if (list.OwnerId != clientId)
            {
                return EitherExtensions.Failure<IListError, ListClient>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));
            }

            if (!await listRepository.IsClientInListAsync(listId, clientIdToRemove))
            {
                return EitherExtensions.Failure<IListError, ListClient>(
                    new ListClientFetchingError.ClientInListNotFound(listId, clientIdToRemove));
            }

            await listRepository.DeleteListClientAsync(listId, clientIdToRemove);
            return EitherExtensions.Success<IListError, ListClient>(new ListClient(
                clientIdToRemove,
                listId
            ));
        });
    }
}