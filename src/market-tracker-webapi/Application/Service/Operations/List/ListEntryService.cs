using market_tracker_webapi.Application.Domain.Models.List;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;
using market_tracker_webapi.Application.Repository.List;
using market_tracker_webapi.Application.Repository.List.ListEntry;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Results;
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
    public async Task<Either<ListFetchingError, ShoppingListEntriesResult>> GetListEntriesAsync(int listId,
        Guid clientId,
        ShoppingListAlternativeType? alternativeType,
        IList<int>? companyIds,
        IList<int>? storeIds,
        IList<int>? cityIds
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ShoppingListEntriesResult>(
                    new ListFetchingError.ListByIdNotFound(listId));

            if (!await listRepository.IsClientInListAsync(list.Id.Value, clientId))
                return EitherExtensions.Failure<ListFetchingError, ShoppingListEntriesResult>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            var listEntries = (await listEntryRepository.GetListEntriesAsync(listId)).ToList();

            ShoppingListEntriesResult? entriesResult;

            switch (alternativeType)
            {
                case ShoppingListAlternativeType.Cheapest:
                    var getEntryDetailsByCriteria = new Func<ListEntry, Task<ListEntryOffer>>(async entry =>
                    {
                        var storeOffer = await priceRepository.GetCheapestStoreOfferAvailableByProductIdAsync(
                            entry.Product.Id.Value, companyIds, storeIds, cityIds);
                        var isAvailable = storeOffer is not null;
                        return new ListEntryOffer(
                            new ProductOffer(
                                entry.Product,
                                storeOffer,
                                isAvailable
                            ),
                            entry.Quantity
                        );
                    });
                    entriesResult = await BuildShoppingListEntriesResult(listEntries, getEntryDetailsByCriteria);
                    break;
                default:
                    entriesResult = await BuildShoppingListEntriesResult(listEntries, async entry =>
                    {
                        var isAvailable = entry.StoreId is not null &&
                                          ((await priceRepository.GetStoreAvailabilityStatusAsync(entry.Product.Id.Value,
                                                  entry.StoreId.Value))
                                              ?.IsAvailable ?? false
                                          );

                        var storeOffer = entry.StoreId is not null && isAvailable
                            ? await priceRepository.GetStoreOfferAsync(entry.Product.Id.Value, entry.StoreId.Value,
                                DateTime.Now)
                            : null;

                        return new ListEntryOffer(
                            new ProductOffer(
                                entry.Product,
                                storeOffer,
                                isAvailable
                            ),
                            entry.Quantity
                        );
                    });
                    break;
            }

            return EitherExtensions.Success<ListFetchingError, ShoppingListEntriesResult>(
                entriesResult
            );
        });
    }

    public async Task<Either<IServiceError, ListEntryId>> AddListEntryAsync(int listId, Guid clientId,
        string productId,
        int storeId, int quantity)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (quantity <= 0)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListEntryCreationError.ListEntryQuantityInvalid(quantity));

            var list = await listRepository.GetListByIdAsync(listId);

            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListFetchingError.ListByIdNotFound(listId));

            if (!await listRepository.IsClientInListAsync(listId, clientId))
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            if (list.ArchivedAt is not null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListUpdateError.ListIsArchived(listId));

            if (await productRepository.GetProductByIdAsync(productId) is null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ProductFetchingError.ProductByIdNotFound(productId));

            if (await storeRepository.GetStoreByIdAsync(storeId) is null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new StoreFetchingError.StoreByIdNotFound(storeId));

            if (await listEntryRepository.GetListEntryAsync(listId, productId) is not null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListEntryCreationError.ProductAlreadyInList(listId, productId)
                );

            var storeAvailability = await priceRepository.GetStoreAvailabilityStatusAsync(productId, storeId);
            if (storeAvailability is null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ProductFetchingError.ProductNotFoundInStore(productId, storeId)
                );
            if (!storeAvailability.IsAvailable)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ProductFetchingError.OutOfStockInStore(productId, storeId)
                );

            var id = await listEntryRepository.AddListEntryAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, ListEntryId>(id);
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


            if (!await listRepository.IsClientInListAsync(listId, clientId))
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

                var storeAvailability = await priceRepository.GetStoreAvailabilityStatusAsync(productId, storeId.Value);
                if (storeAvailability is null)
                    return EitherExtensions.Failure<IServiceError, ListEntry>(
                        new ProductFetchingError.ProductNotFoundInStore(productId, storeId.Value));
                if (!storeAvailability.IsAvailable)
                    return EitherExtensions.Failure<IServiceError, ListEntry>(
                        new ProductFetchingError.OutOfStockInStore(productId, storeId.Value));
            }

            var updatedProductInList =
                await listEntryRepository.UpdateListEntryAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, ListEntry>(updatedProductInList!);
        });
    }

    public async Task<Either<IServiceError, ListEntryId>> DeleteListEntryAsync(int listId, Guid clientId,
        string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(listId);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListFetchingError.ListByIdNotFound(listId));

            if (!await listRepository.IsClientInListAsync(listId, clientId))
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            var listEntry = await listEntryRepository.GetListEntryAsync(listId, productId);
            if (listEntry is null)
                return EitherExtensions.Failure<IServiceError, ListEntryId>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));

            var deleteListEntry = await listEntryRepository.DeleteListEntryAsync(listId, productId);
            return EitherExtensions.Success<IServiceError, ListEntryId>(deleteListEntry!.Id);
        });
    }


    // helper method
    private static async Task<ShoppingListEntriesResult> BuildShoppingListEntriesResult(
        IEnumerable<ListEntry> listEntries, Func<ListEntry, Task<ListEntryOffer>> getEntryDetailsByCriteria
    )
    {
        var listEntriesDetails = new List<ListEntryOffer>();

        var totalPrice = 0;
        var totalProducts = 0;

        foreach (var entry in listEntries)
        {
            var listEntryOffer = await getEntryDetailsByCriteria(entry);

            if (listEntryOffer.ProductOffer.StoreOffer is not null)
            {
                totalPrice += listEntryOffer.ProductOffer.StoreOffer.PriceData.FinalPrice * entry.Quantity;
                totalProducts++;
            }

            listEntriesDetails.Add(listEntryOffer);
        }

        return new ShoppingListEntriesResult
        {
            Products = listEntriesDetails,
            TotalPrice = totalPrice,
            TotalProducts = totalProducts
        };
    }
}