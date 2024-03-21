namespace market_tracker_webapi.Application.Service.Errors.Product;

public class ProductCreationError : ProductError
{
    public class InvalidBrand(string brand) : ProductCreationError
    {
        public string Brand { get; } = brand;
    }

    public class InvalidCategory(string category) : ProductCreationError
    {
        public string Category { get; } = category;
    }
}