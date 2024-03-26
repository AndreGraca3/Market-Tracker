namespace market_tracker_webapi.Application.Service.Errors.Product;

public class ProductFetchingError : ProductError
{
    public class ProductByIdNotFound(int id) : ProductFetchingError
    {
        public int Id { get; } = id;
    }

    public class ProductByBrandNotFound(int brandId) : ProductFetchingError
    {
        public int BrandId { get; } = brandId;
    }
}
