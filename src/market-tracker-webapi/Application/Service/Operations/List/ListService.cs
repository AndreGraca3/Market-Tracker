using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.List;
using market_tracker_webapi.Application.Http.Models.ListEntry;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.User;
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
        string? listName,
        DateTime? createdAfter, bool? isArchived, bool? isOwner = null
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByIdAsync(clientId) is null)
                return EitherExtensions.Failure<IServiceError, CollectionOutputModel<ShoppingList>>(
                    new UserFetchingError.UserByIdNotFound(clientId));

            var lists = await listRepository.GetListsAsync(clientId, listName, createdAfter, isArchived, isOwner);
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
            if (await userRepository.GetUserByIdAsync(clientId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new UserFetchingError.UserByIdNotFound(clientId));

            if (!(await listRepository.GetListsAsync(clientId, listName)).IsNullOrEmpty())
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListCreationError.ListNameAlreadyExists(clientId, listName));

            if ((await listRepository.GetListsAsync(clientId)).Count() >= MaxListNumber)
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

            if (listName is not null && !(await listRepository.GetListsAsync(clientId, listName)).IsNullOrEmpty())
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

    public async Task<Either<IServiceError, ListClient>> AddClientToListAsync(int listId, Guid clientIdToAdd,
        Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByIdAsync(clientId) is null)
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
            return EitherExtensions.Success<IServiceError, ListClient>(new ListClient()
            {
                ClientId = clientIdToAdd,
                ListId = listId
            });
        });
    }

    public async Task<Either<IServiceError, ListClient>> RemoveClientFromListAsync(int listId, Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByIdAsync(clientId) is null)
                return EitherExtensions.Failure<IServiceError, ListClient>(
                    new UserFetchingError.UserByIdNotFound(clientId));

            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListClient>(
                    new ListFetchingError.ListByIdNotFound(listId));

            if (list.OwnerId == clientId)
            {
                await listRepository.DeleteListAsync(listId);
                return EitherExtensions.Success<IServiceError, ListClient>(new ListClient()
                {
                    ClientId = clientId,
                    ListId = listId
                });
            }

            var listClient = await listRepository.DeleteListClientAsync(listId, clientId);
            if (listClient is null)
            {
                return EitherExtensions.Failure<IServiceError, ListClient>(
                    new ListClientFetchingError.ClientInListNotFound(clientId, listId));
            }

            return EitherExtensions.Success<IServiceError, ListClient>(listClient);
        });
    }
}