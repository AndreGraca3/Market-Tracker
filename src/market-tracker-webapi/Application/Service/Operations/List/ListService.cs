using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Repository.Dto.Store;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.Store;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
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
    IStoreRepository storeRepository,
    ITransactionManager transactionManager) : IListService
{
    public async Task<CollectionOutputModel> GetListsAsync(Guid clientId, string? listName, DateTime? archivedAt)
    {
        var lists = await listRepository.GetListsAsync(clientId, listName, archivedAt);
        return new CollectionOutputModel(lists);
    }

    public async Task<Either<ListFetchingError, ListProduct>> GetListByIdAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var list = await listRepository.GetListByIdAsync(id);
            if (list is null)
                return EitherExtensions.Failure<ListFetchingError, ListProduct>(
                    new ListFetchingError.ListByIdNotFound(id));

            var productsInList = await listRepository.GetProductsInListAsync(id);

            var listProduct = await CreateListProduct(list, productsInList);

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
                    new ListCreationError.ListNameOfUserAlreadyExists(clientId, listName));
            
            var id = await listRepository.AddListAsync(clientId, listName);
            return EitherExtensions.Success<IServiceError, IntIdOutputModel>(new IntIdOutputModel(id));;
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
            
            if (list.ClientId != clientId)
                return EitherExtensions.Failure<IServiceError, ListOfProducts>(
                    new ListFetchingError.ListByIdNotFound(id));
            
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

    public async Task<CollectionOutputModel> GetListEntriesAsync(int? listId, string? productId, int? storeId, int? quantity)
    {
        var productsInList = await listRepository.GetProductsInListAsync(listId, productId, storeId, quantity);
        return new CollectionOutputModel(productsInList);
    }

    public async Task<Either<IServiceError, ListEntry>> GetListEntryByIdAsync(int listId, string productId, int storeId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var productInList = await listRepository.GetProductsByListIdAsync(listId, productId, storeId);
            if (productInList is null)
                return EitherExtensions.Failure<IServiceError, ListEntry>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId, storeId));
            
            var listEntry = new ListEntry
            {
                ProductItem = new ProductItem
                {
                    ProductId = productInList.ProductId,
                    Name = (await productRepository.GetProductByIdAsync(productInList.ProductId))!.Name
                },
                Quantity = productInList.Quantity,
                StorePrice = await priceRepository.GetStorePriceByProductIdAsync(productInList.ProductId, productInList.StoreId, DateTime.Now)
            };
            
            return EitherExtensions.Success<IServiceError, ListEntry>(listEntry);

        });
    }

    public async Task<Either<IServiceError, IntIdOutputModel>> AddListEntryAsync(int listId, string productId, int storeId, int quantity)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await listRepository.GetListByIdAsync(listId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListFetchingError.ListByIdNotFound(listId));
            
            if (await productRepository.GetProductByIdAsync(productId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId));
            
            if (await storeRepository.GetStoreByIdAsync(storeId) is null)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(storeId));

            if (quantity <= 0)
                return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new ListEntryCreationError.ListEntryQuantityInvalid(quantity));

            var id = await listRepository.AddProductInListAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, IntIdOutputModel>(new IntIdOutputModel(id));
        });
    }

    public async Task<Either<IServiceError, ProductInList>> UpdateListEntryAsync(int listId, string productId, int storeId, int? quantity = null)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var productInList = await listRepository.GetProductsByListIdAsync(listId, productId, storeId);
            if (productInList is null)
                return EitherExtensions.Failure<IServiceError, ProductInList>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId, storeId));
            
            if (quantity <= 0)
                return EitherExtensions.Failure<IServiceError, ProductInList>(
                    new ListEntryCreationError.ListEntryQuantityInvalid(quantity));

            var updatedProductInList = await listRepository.UpdateProductInListAsync(listId, productId, storeId, quantity);
            return EitherExtensions.Success<IServiceError, ProductInList>(updatedProductInList!);
        });
    }

    public async Task<Either<ListEntryFetchingError, ProductInList>> DeleteListEntryAsync(int listId, string productId, int storeId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var productInList = await listRepository.GetProductsByListIdAsync(listId, productId, storeId);
            if (productInList is null)
                return EitherExtensions.Failure<ListEntryFetchingError, ProductInList>(
                    new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId, storeId));
            
            var deletedProductInList = await listRepository.DeleteProductInListAsync(listId, productId, storeId);
            return EitherExtensions.Success<ListEntryFetchingError, ProductInList>(deletedProductInList!);
        });
    }
    
    private async Task<ListProduct> CreateListProduct(ListOfProducts list, IEnumerable<ProductInList> productsInList)
    {
        var listEntries = new List<ListEntry>();

        foreach (var product in productsInList)
        {
            var productDetail = await productRepository.GetProductByIdAsync(product.ProductId);

            var productItem = new ProductItem
            {
                ProductId = product.ProductId,
                Name = productDetail!.Name
            };

            var storePrice = await priceRepository.GetStorePriceByProductIdAsync(product.ProductId, product.StoreId, DateTime.Now);

            var listEntry = new ListEntry
            {
                ProductItem = productItem,
                Quantity = product.Quantity,
                StorePrice = storePrice
            };

            listEntries.Add(listEntry);
        }

        var totalPrice = listEntries.Sum(entry => entry.StorePrice.PriceData.Price * entry.Quantity);
        var totalProducts = listEntries.Sum(entry => entry.Quantity);

        return new ListProduct
        {
            Id = list.Id,
            Name = list.ListName,
            ArchivedAt = list.ArchivedAt,
            CreatedAt = list.CreatedAt,
            Products = listEntries,
            TotalPrice = totalPrice,
            TotalProducts = totalProducts
        };
    }
}