using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
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
    IPriceRepository priceRepository,
    IProductRepository productRepository,
    IListEntryRepository listEntryRepository,
    ITransactionManager transactionManager) : IListService
{
    private const int MaxListNumber = 10;

    public async Task<Either<IServiceError, CollectionOutputModel>> GetListsAsync(Guid clientId, string? listName,
        DateTime? createdAfter, bool? isArchived, bool? isOwner = null
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByIdAsync(clientId) is null)
                return EitherExtensions.Failure<IServiceError, CollectionOutputModel>(
                    new UserFetchingError.UserByIdNotFound(clientId));
            
            var lists = await listRepository.GetListsAsync(clientId, listName, createdAfter, isArchived, isOwner);
            return EitherExtensions.Success<IServiceError, CollectionOutputModel>(new CollectionOutputModel(lists));
        });
    }

    public async Task<Either<ListFetchingError, ListProduct>> GetListByIdAsync(int id, Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ListProduct>(
                    new ListFetchingError.ListByIdNotFound(id));

            var listClients = (await listRepository.GetListClientsByListIdAsync(id)).ToList();
            if (!listClients.Contains(clientId))
                return EitherExtensions.Failure<ListFetchingError, ListProduct>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, id)
                );

            var listEntries = await listEntryRepository.GetListEntriesAsync(id);

            var listProduct = await CreateListProduct(list, listEntries, listClients);

            return EitherExtensions.Success<ListFetchingError, ListProduct>(listProduct);
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

    public async Task<Either<IServiceError, ListOfProducts>> UpdateListAsync(int id, Guid clientId, string? listName,
        bool? isArchived
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListFetchingError.ListByIdNotFound(id));
            
            if (list.OwnerId != clientId)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, id));
                
            if (list.ArchivedAt != null)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListUpdateError.ListIsArchived(id)
                );

            if (listName is not null && !(await listRepository.GetListsAsync(clientId, listName)).IsNullOrEmpty())
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListCreationError.ListNameAlreadyExists(clientId, listName)
                );

            DateTime? archivedAt = isArchived.HasValue && isArchived.Value ? DateTime.Now : null;
            
            var updatedList = await listRepository.UpdateListAsync(id, archivedAt, listName);
            return EitherExtensions.Success<IServiceError, ListOfProducts>(updatedList!);
        });
    }

    public async Task<Either<ListFetchingError, ListOfProducts>> DeleteListAsync(int id, Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ListOfProducts>(
                    new ListFetchingError.ListByIdNotFound(id));

            if (list.OwnerId != clientId)
                return EitherExtensions.Failure<ListFetchingError, ListOfProducts>(
                    new ListFetchingError.UserDoesNotOwnList(clientId, id));

            var deletedList = await listRepository.DeleteListAsync(id);
            return EitherExtensions.Success<ListFetchingError, ListOfProducts>(deletedList!);
        });
    }
    public async Task<Either<IServiceError, ListClient>> AddClientToListAsync(int listId, Guid clientIdToAdd, Guid clientId)
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

            if ((await listRepository.GetListClientsByListIdAsync(listId)).Contains(clientIdToAdd))
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
    
    private async Task<ListProduct> CreateListProduct(ListOfProducts list, IEnumerable<ListEntry> listEntries, IEnumerable<Guid> clientIds)
    {
        var listEntriesDetails = new List<ListEntryDetails>();

        foreach (var product in listEntries)
        {
            var productDetail = await productRepository.GetProductByIdAsync(product.ProductId);

            var productItem = new ProductItem
            {
                ProductId = product.ProductId,
                Name = productDetail!.Name
            };

            var storePrice =
                await priceRepository.GetStorePriceByProductIdAsync(product.ProductId, product.StoreId, DateTime.Now);

            var storeAvailability = await priceRepository.GetStoreAvailabilityAsync(product.ProductId, product.StoreId);

            var listEntry = new ListEntryDetails
            {
                ProductItem = productItem,
                Quantity = product.Quantity,
                StorePrice = storePrice,
                IsAvailable = storeAvailability is not null && storeAvailability.IsAvailable
            };

            listEntriesDetails.Add(listEntry);
        }

        var totalPrice = listEntriesDetails.Sum(entry => entry.StorePrice.PriceData.Price * entry.Quantity);
        var totalProducts = listEntriesDetails.Sum(entry => entry.Quantity);

        return new ListProduct
        {
            Id = list.Id,
            Name = list.ListName,
            ArchivedAt = list.ArchivedAt,
            CreatedAt = list.CreatedAt,
            ClientIds = clientIds,
            Products = listEntriesDetails,
            TotalPrice = totalPrice,
            TotalProducts = totalProducts
        };
    }
}