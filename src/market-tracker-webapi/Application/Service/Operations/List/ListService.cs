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
    public async Task<Either<IServiceError, CollectionOutputModel>> GetListsAsync(Guid clientId, string? listName, DateTime? archivedAt)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var lists = await listRepository.GetListsAsync(clientId, listName, archivedAt);
            return EitherExtensions.Success<IServiceError, CollectionOutputModel>(new CollectionOutputModel(lists));
        });
    }

    public async Task<Either<ListFetchingError, ListProduct>> GetListByIdAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ListProduct>(
                    new ListFetchingError.ListByIdNotFound(id));

            var listEntries = await listEntryRepository.GetListEntriesAsync(id);

            var listProduct = await CreateListProduct(list, listEntries);

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
            
            var id = await listRepository.AddListAsync(clientId, listName);
            return EitherExtensions.Success<IServiceError, IntIdOutputModel>(new IntIdOutputModel(id));
        });
    }

    public async Task<Either<IServiceError, ListOfProducts>> UpdateListAsync(int id, Guid clientId, string? listName, DateTime? archivedAt)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListFetchingError.ListByIdNotFound(id));
            
            if (list.ArchivedAt != null)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListUpdateError.ListIsArchived(id));
            
            if (list.ClientId != clientId)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new UserPermissionsError.UserDoNotOwnList(clientId, id));
            
            var updatedList = await listRepository.UpdateListAsync(id, listName, archivedAt);
            return EitherExtensions.Success<IServiceError, ListOfProducts>(updatedList!);
        });
    }

    public async Task<Either<ListFetchingError, ListOfProducts>> DeleteListAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ListOfProducts>(
                    new ListFetchingError.ListByIdNotFound(id));
            
            var deletedList = await listRepository.DeleteListAsync(id);
            return EitherExtensions.Success<ListFetchingError, ListOfProducts>(deletedList!);
        });
    }
    
    private async Task<ListProduct> CreateListProduct(ListOfProducts list, IEnumerable<ListEntry> listEntries)
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

            var storePrice = await priceRepository.GetStorePriceByProductIdAsync(product.ProductId, product.StoreId, DateTime.Now);
            
            var isAvailable = !(await priceRepository.GetStoresAvailabilityAsync(product.ProductId, product.StoreId)).IsNullOrEmpty();
            
            var listEntry = new ListEntryDetails
            {
                ProductItem = productItem,
                Quantity = product.Quantity,
                StorePrice = storePrice,
                IsAvailable = isAvailable
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
            Products = listEntriesDetails,
            TotalPrice = totalPrice,
            TotalProducts = totalProducts
        };
    }
}