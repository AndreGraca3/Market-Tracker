namespace market_tracker_webapi.Application.Service.Errors.Product;

public class ProductFetchingError : IProductError
{
    public class ProductByIdNotFound(string id) : ProductFetchingError
    {
        public string Id { get; } = id;
    }
    
    public class OutOfStockInStore(string productId, int storeId) : ProductFetchingError
    {
        public string ProductId { get; } = productId;
        public int StoreId { get; } = storeId;
    }

    public class ProductNotFoundInStore(string productId, int storeId) : ProductFetchingError
    {
        public string ProductId { get; } = productId;
        public int StoreId { get; } = storeId;
    }
    
    public class PriceAlertNotFound(Guid clientId, string productId) : ProductFetchingError
    {
        public string ProductId { get; } = productId;
        public Guid ClientId { get; } = clientId;
    }
}