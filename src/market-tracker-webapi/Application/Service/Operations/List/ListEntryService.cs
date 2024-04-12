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
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.List;

public class ListEntryService(    
    IListRepository listRepository, 
    IListEntryRepository listEntryRepository,
    IPriceRepository priceRepository,
    IProductRepository productRepository,
    IStoreRepository storeRepository,
    ITransactionManager transactionManager) : IListEntryService
{
    public async Task<Either<IServiceError, ListEntryDetails>> GetListEntryByIdAsync(int listId, string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var listEntry = await listEntryRepository.GetListEntriesByListIdAsync(listId, productId);
            if (listEntry is null)
                return EitherExtensions.Failure<IServiceError, ListEntryDetails>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));
            
            var isAvailable = !(await priceRepository.GetStoresAvailabilityAsync(listEntry.ProductId, listEntry.StoreId)).IsNullOrEmpty();
            
            var listEntryDetails = new ListEntryDetails
            {
                ProductItem = new ProductItem
                {
                    ProductId = listEntry.ProductId,
                    Name = (await productRepository.GetProductByIdAsync(listEntry.ProductId))!.Name
                },
                Quantity = listEntry.Quantity,
                StorePrice = await priceRepository.GetStorePriceByProductIdAsync(listEntry.ProductId, listEntry.StoreId, DateTime.Now),
                IsAvailable = isAvailable
            };
            
            return EitherExtensions.Success<IServiceError, ListEntryDetails>(listEntryDetails);

        });
    }

    public async Task<Either<IServiceError, IntIdOutputModel>> AddListEntryAsync(int listId, string productId, int storeId, int quantity)
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
            
            if (list.ArchivedAt is not null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListUpdateError.ListIsArchived(listId));
            
            if (await productRepository.GetProductByIdAsync(productId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId));
            
            if (await storeRepository.GetStoreByIdAsync(storeId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(storeId));
            
            if ((await priceRepository.GetStoresAvailabilityAsync(productId, storeId)).IsNullOrEmpty())
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ProductFetchingError.UnavailableProductInStore(productId, storeId));

            var id = await listEntryRepository.AddListEntryAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, IntIdOutputModel>(new IntIdOutputModel(id));
        });
    }

    public async Task<Either<IServiceError, ListEntry>> UpdateListEntryAsync(int listId, string productId, int? storeId, int? quantity = null)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var listEntry = await listEntryRepository.GetListEntriesByListIdAsync(listId, productId);
            if (listEntry is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));
            
            if (storeId is not null && (await priceRepository.GetStoresAvailabilityAsync(productId, storeId.Value)).IsNullOrEmpty())
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ProductFetchingError.UnavailableProductInStore(productId, storeId.Value));
            
            if (quantity <= 0)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListEntryCreationError.ListEntryQuantityInvalid(quantity));

            var updatedProductInList = await listEntryRepository.UpdateListEntryAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, ListEntry>(updatedProductInList!);
        });
    }

    public async Task<Either<ListEntryFetchingError, ListEntry>> DeleteListEntryAsync(int listId, string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var listEntry = await listEntryRepository.GetListEntriesByListIdAsync(listId, productId);
            if (listEntry is null)
                return EitherExtensions.Failure<ListEntryFetchingError, ListEntry>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));
            
            var deleteListEntry = await listEntryRepository.DeleteListEntryAsync(listId, productId);
            return EitherExtensions.Success<ListEntryFetchingError, ListEntry>(deleteListEntry!);
        });
    }
    
}