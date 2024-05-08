using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.List;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.List;
using market_tracker_webapi.Application.Http.Models.ListEntry;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Operations.Market.Price;
using market_tracker_webapi.Application.Repository.Operations.Market.Store;
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

            if (!await listRepository.IsClientInListAsync(listId, clientId))
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

            var storeAvailability = await priceRepository.GetStoreAvailabilityStatusAsync(productId, storeId);
            if (storeAvailability is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ProductFetchingError.ProductNotFoundInStore(productId, storeId)
                );
            if (!storeAvailability.IsAvailable)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ProductFetchingError.OutOfStockInStore(productId, storeId)
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

    public async Task<Either<IServiceError, ListEntry>> DeleteListEntryAsync(int listId, Guid clientId,
        string productId)
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

            var listEntry = await listEntryRepository.GetListEntryAsync(listId, productId);
            if (listEntry is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));

            var deleteListEntry = await listEntryRepository.DeleteListEntryAsync(listId, productId);
            return EitherExtensions.Success<IServiceError, ListEntry>(deleteListEntry!);
        });
    }

    public async Task<Either<ListFetchingError, ShoppingListEntriesOutputModel>> GetListEntriesAsync(int listId,
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
                return EitherExtensions.Failure<ListFetchingError, ShoppingListEntriesOutputModel>(
                    new ListFetchingError.ListByIdNotFound(listId));

            if (!await listRepository.IsClientInListAsync(list.Id, clientId))
                return EitherExtensions.Failure<ListFetchingError, ShoppingListEntriesOutputModel>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, listId));

            var listEntries = (await listEntryRepository.GetListEntriesAsync(listId)).ToList();

            ShoppingListEntriesOutputModel? entriesResult = null;

            switch (alternativeType)
            {
                case ShoppingListAlternativeType.Cheapest:
                    var getEntryDetailsByCriteria = new Func<ListEntry, Task<ListEntryDetails>>(async entry =>
                    {
                        var storePrice = await priceRepository.GetCheapestStorePriceAvailableByProductIdAsync(
                            entry.ProductId, companyIds, storeIds, cityIds);
                        var isAvailable = storePrice is not null;
                        return new ListEntryDetails
                        {
                            ProductItem = new ProductItem
                            {
                                ProductId = entry.ProductId,
                                Name = (await productRepository.GetProductByIdAsync(entry.ProductId))!.Name
                            },
                            Quantity = entry.Quantity,
                            StorePrice = storePrice,
                            IsAvailable = isAvailable
                        };
                    });
                    entriesResult = await BuildShoppingListEntriesResult(listEntries, getEntryDetailsByCriteria);
                    break;
                default:
                    entriesResult = await BuildShoppingListEntriesResult(listEntries, async entry =>
                    {
                        var isAvailable = entry.StoreId is not null && ((await priceRepository.GetStoreAvailabilityStatusAsync(entry.ProductId, entry.StoreId.Value))
                            ?.IsAvailable ?? false);

                        var storePrice = entry.StoreId is not null && isAvailable
                            ? await priceRepository.GetStorePriceAsync(entry.ProductId, entry.StoreId.Value,
                                DateTime.Now)
                            : null;

                        return new ListEntryDetails
                        {
                            ProductItem = new ProductItem
                            {
                                ProductId = entry.ProductId,
                                Name = (await productRepository.GetProductByIdAsync(entry.ProductId))!.Name
                            },
                            Quantity = entry.Quantity,
                            StorePrice = storePrice,
                            IsAvailable = isAvailable
                        };
                    });
                    break;
            }

            return EitherExtensions.Success<ListFetchingError, ShoppingListEntriesOutputModel>(
                entriesResult
            );
        });
    }

    // helper method
    private static async Task<ShoppingListEntriesOutputModel> BuildShoppingListEntriesResult(
        IEnumerable<ListEntry> listEntries, Func<ListEntry, Task<ListEntryDetails>> getEntryDetailsByCriteria
    )
    {
        var listEntriesDetails = new List<ListEntryDetails>();

        var totalPrice = 0;
        var totalProducts = 0;

        foreach (var product in listEntries)
        {
            var listEntry = await getEntryDetailsByCriteria(product);

            if (listEntry.StorePrice is not null)
            {
                totalPrice += listEntry.StorePrice.PriceData.FinalPrice * product.Quantity;
                totalProducts++;
            }

            listEntriesDetails.Add(listEntry);
        }

        return new ShoppingListEntriesOutputModel
        {
            Products = listEntriesDetails,
            TotalPrice = totalPrice,
            TotalProducts = totalProducts
        };
    }
}