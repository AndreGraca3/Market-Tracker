using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.EntityFramework;

public class BrandRepository(MarketTrackerDataContext dataContext) : IBrandRepository
{
    public async Task<Brand?> GetBrandByIdAsync(int brandId)
    {
        return await dataContext.Brand.FindAsync(brandId) is null
            ? null
            : new Brand(brandId, "brandName");
    }

    public async Task<Brand?> GetBrandByNameAsync(string brandName)
    {
        var brandEntity = await dataContext.Brand.FirstOrDefaultAsync(brand =>
            brand.Name == brandName
        );
        return brandEntity is null ? null : new Brand(brandEntity.Id, brandEntity.Name);
    }

    public async Task<Brand> AddBrandAsync(string name)
    {
        var brandEntity = new BrandEntity() { Name = name };
        await dataContext.Brand.AddAsync(brandEntity);
        await dataContext.SaveChangesAsync();
        return new Brand(brandEntity.Id, brandEntity.Name);
    }

    public async Task<Brand?> RemoveBrandAsync(int brandId)
    {
        var brandEntity = await dataContext.Brand.FindAsync(brandId);
        if (brandEntity is null)
        {
            return null;
        }
        dataContext.Brand.Remove(brandEntity);
        await dataContext.SaveChangesAsync();
        return new Brand(brandEntity.Id, brandEntity.Name);
    }
}
