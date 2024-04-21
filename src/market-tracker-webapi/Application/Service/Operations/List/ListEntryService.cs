using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.List;

public class ListEntryService(
    IListRepository listRepository,
    IListEntryRepository listEntryRepository,
    IPriceRepository priceRepository,
    IProductRepository productRepository,
    IStoreRepository storeRepository,
    ITransactionManager transactionManager) : IListEntryService
{
    public async Task<Either<IServiceError, IntIdOutputModel>> AddListEntryAsync(int listId, Guid clientId,
        string productId,
        int storeId, int quantity)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (quantity <= 0)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListEntryCreationError.ListEntryQuantityInvalid(quantity));

            var list = await listRepository.GetListByIdAsync(listId);

            if (list is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListFetchingError.ListByIdNotFound(listId));

            var listClients = await listRepository.GetListClientsByListIdAsync(listId);
            if (!listClients.Contains(clientId))
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            if (list.ArchivedAt is not null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListUpdateError.ListIsArchived(listId));

            if (await productRepository.GetProductByIdAsync(productId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId));

            if (await storeRepository.GetStoreByIdAsync(storeId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(storeId));

            if (await listEntryRepository.GetListEntryAsync(listId, productId) is not null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListEntryCreationError.ProductAlreadyInList(listId, productId)
                );

            var storeAvailability = await priceRepository.GetStoreAvailabilityAsync(productId, storeId);
            if (storeAvailability is null || !storeAvailability.IsAvailable)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ProductFetchingError.UnavailableProductInStore(productId, storeId)
                );

            var id = await listEntryRepository.AddListEntryAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, IntIdOutputModel>(new IntIdOutputModel(id));
        });
    }

    public async Task<Either<IServiceError, ListEntry>> UpdateListEntryAsync(int listId, Guid clientId,
        string productId, int? storeId,
        int? quantity = null)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListFetchingError.ListByIdNotFound(listId));

            
            var listClients = await listRepository.GetListClientsByListIdAsync(listId);
            if (!listClients.Contains(clientId))
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            if (quantity <= 0)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListEntryCreationError.ListEntryQuantityInvalid(quantity));

            if (await productRepository.GetProductByIdAsync(productId) is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ProductFetchingError.ProductByIdNotFound(productId));

            var listEntry = await listEntryRepository.GetListEntryAsync(listId, productId);
            if (listEntry is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));

            if (storeId is not null)
            {
                if (await storeRepository.GetStoreByIdAsync(storeId.Value) is null)
                    return EitherExtensions.Failure<IServiceError, ListEntry>(
                        new StoreFetchingError.StoreByIdNotFound(storeId.Value));

                var storeAvailability = await priceRepository.GetStoreAvailabilityAsync(productId, storeId.Value);
                if (storeAvailability is null || !storeAvailability.IsAvailable)
                    return EitherExtensions.Failure<IServiceError, ListEntry>(
                        new ProductFetchingError.UnavailableProductInStore(productId, storeId.Value));
            }

            var updatedProductInList =
                await listEntryRepository.UpdateListEntryAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, ListEntry>(updatedProductInList!);
        });
    }

    public async Task<Either<IServiceError, ListEntry>> DeleteListEntryAsync(int listId, Guid clientId,
        string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListFetchingError.ListByIdNotFound(listId));

            var listClients = await listRepository.GetListClientsByListIdAsync(listId);
            if (!listClients.Contains(clientId))
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            var listEntry = await listEntryRepository.GetListEntryAsync(listId, productId);
            if (listEntry is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));

            var deleteListEntry = await listEntryRepository.DeleteListEntryAsync(listId, productId);
            return EitherExtensions.Success<IServiceError, ListEntry>(deleteListEntry!);
        });
    }
}