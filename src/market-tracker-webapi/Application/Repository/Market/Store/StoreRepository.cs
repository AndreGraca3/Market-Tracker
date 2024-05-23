using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Store;

using Store = Domain.Schemas.Market.Retail.Shop.Store;

public class StoreRepository(MarketTrackerDataContext dataContext) : IStoreRepository
{
    public async Task<IEnumerable<Store>> GetStoresAsync(int? companyId, int? cityId, string? storeName)
    {
        var query = from store in dataContext.Store
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            where (companyId == null || store.CompanyId == companyId) &&
                  (cityId == null || store.CityId == cityId)
                  && (storeName == null || EF.Functions.ILike(store.Name, $"%{storeName}%"))
            select new
            {
                StoreEntity = store,
                CityEntity = city,
                CompanyEntity = company
            };

        return await query.Select(
            s => s.StoreEntity.ToStore(s.CityEntity == null ? null : s.CityEntity.ToCity(),
                s.CompanyEntity.ToCompany())).ToListAsync();
    }

    public async Task<Store?> GetStoreByIdAsync(int id)
    {
        var query = from store in dataContext.Store
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            where store.Id == id
            select new
            {
                StoreEntity = store,
                CityEntity = city,
                CompanyEntity = company
            };

        return await query.Select(
            s => s.StoreEntity.ToStore(s.CityEntity == null ? null : s.CityEntity.ToCity(),
                s.CompanyEntity.ToCompany())).FirstOrDefaultAsync();
    }

    public async Task<Store?> GetStoreByNameAsync(string name)
    {
        var query = from store in dataContext.Store
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            where EF.Functions.ILike(store.Name, $"%{name}%")
            select new
            {
                StoreEntity = store,
                CityEntity = city,
                CompanyEntity = company
            };

        return await query.Select(
            s => s.StoreEntity.ToStore(s.CityEntity == null ? null : s.CityEntity.ToCity(),
                s.CompanyEntity.ToCompany())).FirstOrDefaultAsync();
    }

    public async Task<Store?> GetStoreByOperatorIdAsync(Guid operatorId)
    {
        var query = from store in dataContext.Store
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            where store.OperatorId == operatorId
            select new
            {
                StoreEntity = store,
                CityEntity = city,
                CompanyEntity = company
            };

        return await query.Select(
            s => s.StoreEntity.ToStore(s.CityEntity == null ? null : s.CityEntity.ToCity(),
                s.CompanyEntity.ToCompany())).FirstOrDefaultAsync();
    }

    public async Task<StoreId> AddStoreAsync(string name, string address, int? cityId, int companyId, Guid operatorId)
    {
        var newStore = new StoreEntity
        {
            Name = name,
            Address = address,
            CityId = cityId,
            CompanyId = companyId,
            OperatorId = operatorId
        };

        dataContext.Store.Add(newStore);
        await dataContext.SaveChangesAsync();

        return new StoreId(newStore.Id);
    }

    public async Task<StoreItem?> UpdateStoreAsync(
        int id,
        string address,
        int cityId,
        int companyId
    )
    {
        var storeEntity = await dataContext.Store.FindAsync(id);

        if (storeEntity == null)
        {
            return null;
        }

        storeEntity.Address = address;
        storeEntity.CityId = cityId;
        storeEntity.CompanyId = companyId;

        await dataContext.SaveChangesAsync();
        return storeEntity.ToStoreItem();
    }

    public async Task<StoreItem?> DeleteStoreAsync(int id)
    {
        var storeEntity = await dataContext.Store.FindAsync(id);

        if (storeEntity == null)
        {
            return null;
        }

        dataContext.Store.Remove(storeEntity);
        await dataContext.SaveChangesAsync();

        return storeEntity.ToStoreItem();
    }
}