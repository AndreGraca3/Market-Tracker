namespace market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

public record Product(
    ProductId Id,
    string Name,
    string ImageUrl,
    int Quantity,
    ProductUnit Unit,
    int Views,
    double Rating,
    Brand Brand,
    Category Category
)
{
    public Product(
        string Id,
        string Name,
        string ImageUrl,
        int Quantity,
        ProductUnit Unit,
        int Views,
        double Rating,
        Brand Brand,
        Category Category
    ) : this(
        new ProductId(Id),
        Name,
        ImageUrl,
        Quantity,
        Unit,
        Views,
        Rating,
        Brand,
        Category
    )
    {
    }
}

public record ProductItem(string ProductId, string Name, string ImageUrl, string BrandName);

public record ProductId(string Value);