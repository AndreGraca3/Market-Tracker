namespace market_tracker_webapi.Application.Repository.Dto;

public class ListProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ProductStats> Products { get; set; }
}

public class ProductStats
{
    public StoreProduct StoreProduct { get; set; }
    public int Quantity { get; set; }
}

public class StoreProduct
{
    public Product Product { get; set; }
    public Store Store { get; set; }
}

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public PriceData PriceData { get; set; }
}

public class PriceData
{
    public decimal Price { get; set; }
    public decimal Promotion { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Company Company { get; set; }
}

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Changes