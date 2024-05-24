namespace market_tracker_webapi.Application.Repository.Market.Inventory.Brand;

using Brand = Domain.Schemas.Market.Inventory.Brand;

public interface IBrandRepository
{
    Task<Brand?> GetBrandByIdAsync(int brandId);

    Task<Brand?> GetBrandByNameAsync(string brandName);

    Task<Brand> AddBrandAsync(string name);

    Task<Brand?> RemoveBrandAsync(int brandId);
}
