namespace market_tracker_webapi.Application.Service.Errors.Product;

public class ProductFetchingError : IProductError
{
    public class ProductByIdNotFound(string id) : ProductFetchingError
    {
        public string Id { get; } = id;
    }
}
