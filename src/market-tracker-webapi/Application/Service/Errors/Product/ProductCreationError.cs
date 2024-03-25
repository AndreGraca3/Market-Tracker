namespace market_tracker_webapi.Application.Service.Errors.Product;

public class ProductCreationError : IProductError
{
    public class InvalidBrand(string brandName) : ProductCreationError
    {
        public string BrandName { get; } = brandName;
    }
    
    public class ProductAlreadyExists(int productId) : ProductCreationError
    {
        public int ProductId { get; } = productId;
    }
}
