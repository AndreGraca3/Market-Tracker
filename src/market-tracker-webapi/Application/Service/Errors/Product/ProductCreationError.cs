namespace market_tracker_webapi.Application.Service.Errors.Product;

public class ProductCreationError : IProductError
{
    public class InvalidBrand(string brandName) : ProductCreationError
    {
        public string BrandName { get; } = brandName;
    }

    public class ProductAlreadyExists(string productId) : ProductCreationError
    {
        public string ProductId { get; } = productId;
    }
}
