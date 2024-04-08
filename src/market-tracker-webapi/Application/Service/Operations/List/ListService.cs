using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.List;

public class ListService(
    IListRepository listRepository, 
    IUserRepository userRepository,
    ITransactionManager transactionManager) : IListService
{
    public async Task<CollectionOutputModel> GetListsOfProductsAsync(Guid? clientId, string? listName, DateTime? archivedAt)
    {
        var lists = await listRepository.GetListsOfProductsAsync(clientId, listName, archivedAt);
        return new CollectionOutputModel(lists);
    }

    public async Task<Either<ListFetchingError, ListOfProducts>> GetListOfProductsByIdAsync(int id)
    {
        var list = await listRepository.GetListOfProductsByIdAsync(id);
        return list is null
            ? EitherExtensions.Failure<ListFetchingError, ListOfProducts>(
                new ListFetchingError.ListByIdNotFound(id)
            )
            : EitherExtensions.Success<ListFetchingError, ListOfProducts>(list);
    }

    public async Task<Either<IServiceError, int>> AddListOfProductsAsync(Guid clientId, string listName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByIdAsync(clientId) is null)
                return EitherExtensions.Failure<IServiceError, int>(
                    new UserFetchingError.UserByIdNotFound(clientId));
            
            var id = await listRepository.AddListOfProductsAsync(clientId, listName);
            return EitherExtensions.Success<IServiceError, int>(id);
        });
    }

    public async Task<Either<IServiceError, ListOfProducts>> UpdateListOfProductsAsync(int id, Guid clientId, string? listName, DateTime? archivedAt)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByIdAsync(clientId) is null)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new UserFetchingError.UserByIdNotFound(clientId));
            
            var list = await listRepository.GetListOfProductsByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListFetchingError.ListByIdNotFound(id));
            
            var updatedList = await listRepository.UpdateListOfProductsAsync(id, listName, archivedAt);

            return EitherExtensions.Success<IServiceError, ListOfProducts>(updatedList!);
        });
    }

    public async Task<Either<ListFetchingError, ListOfProducts>> DeleteListOfProductsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CollectionOutputModel> GetProductsInListAsync(int? listId, int? productId, int? storeId, int? quantity)
    {
        throw new NotImplementedException();
    }

    public async Task<Either<IServiceError, ProductInList>> GetProductsByListIdAsync(int listId, int productId, int storeId)
    {
        throw new NotImplementedException();
    }

    public async Task<Either<IServiceError, int>> AddProductInListAsync(int listId, int productId, int storeId, int quantity)
    {
        throw new NotImplementedException();
    }

    public async Task<Either<IServiceError, ProductInList>> UpdateProductInListAsync(int listId, int productId, int storeId, int? quantity = null)
    {
        throw new NotImplementedException();
    }

    public async Task<Either<IServiceError, ProductInList>> DeleteProductInListAsync(int listId, int productId, int storeId)
    {
        throw new NotImplementedException();
    }
}