namespace market_tracker_webapi.Application.Repository.Operations.Brand;

using Brand = Domain.Brand;

public interface IBrandRepository
{
    Task<Brand?> GetBrandByIdAsync(int brandId);

    Task<Brand?> GetBrandByNameAsync(string brandName);

    Task<Brand> AddBrandAsync(string name);

    Task<Brand?> RemoveBrandAsync(int brandId);
}
