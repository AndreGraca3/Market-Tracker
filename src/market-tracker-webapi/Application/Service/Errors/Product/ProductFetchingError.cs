namespace market_tracker_webapi.Application.Service.Errors.Product;

public class ProductFetchingError : IProductError
{
    public class ProductByIdNotFound(int id) : ProductFetchingError
    {
        public int Id { get; } = id;
    }
}
