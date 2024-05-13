using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Market.Store;

using Store = Domain.Schemas.Market.Retail.Shop.Store;

public class StoreService(
    IStoreRepository storeRepository,
    ICityRepository cityRepository,
    ICompanyRepository companyRepository,
    ITransactionManager transactionManager
) : IStoreService
{
    public async Task<IEnumerable<Store>> GetStoresAsync(
        int? companyId = null,
        int? cityId = null,
        string? name = null
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
            await storeRepository.GetStoresAsync(companyId, cityId, name)
        );
    }

    public async Task<Store> GetStoreByIdAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await storeRepository.GetStoreByIdAsync(id) ??
            throw new MarketTrackerServiceException(new StoreFetchingError.StoreByIdNotFound(id)));
    }

    public async Task<StoreId> AddStoreAsync(
        string name,
        string address,
        int? cityId,
        int companyId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await storeRepository.GetStoreByNameAsync(name) is not null)
            {
                throw new MarketTrackerServiceException(
                    new StoreCreationError.StoreNameAlreadyExists(name)
                );
            }

            if (cityId.HasValue && await cityRepository.GetCityByIdAsync(cityId.Value) is null)
            {
                throw new MarketTrackerServiceException(
                    new CityFetchingError.CityByIdNotFound(cityId.Value)
                );
            }

            if (await companyRepository.GetCompanyByIdAsync(companyId) is null)
            {
                throw new MarketTrackerServiceException(
                    new CompanyFetchingError.CompanyByIdNotFound(companyId)
                );
            }

            return await storeRepository.AddStoreAsync(name, address, cityId, companyId);
        });
    }

    public async Task<StoreItem> UpdateStoreAsync(
        int id,
        string name,
        string address,
        int cityId,
        int companyId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var store = await storeRepository.GetStoreByIdAsync(id);
            if (store is null)
            {
                throw new MarketTrackerServiceException(
                    new StoreFetchingError.StoreByIdNotFound(id)
                );
            }

            if (await storeRepository.GetStoreByNameAsync(name) is not null)
            {
                throw new MarketTrackerServiceException(
                    new StoreCreationError.StoreNameAlreadyExists(name)
                );
            }

            if (await cityRepository.GetCityByIdAsync(cityId) is null)
            {
                throw new MarketTrackerServiceException(
                    new CityFetchingError.CityByIdNotFound(cityId)
                );
            }

            if (await companyRepository.GetCompanyByIdAsync(companyId) is null)
            {
                throw new MarketTrackerServiceException(
                    new CompanyFetchingError.CompanyByIdNotFound(companyId)
                );
            }

            var updatedStore = await storeRepository.UpdateStoreAsync(
                id,
                address,
                cityId,
                companyId
            );
            return updatedStore ?? throw new MarketTrackerServiceException(
                new StoreFetchingError.StoreByIdNotFound(id)
            );
        });
    }

    public async Task<StoreId> DeleteStoreAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            (await storeRepository.DeleteStoreAsync(id))?.Id ?? throw new MarketTrackerServiceException(
                new StoreFetchingError.StoreByIdNotFound(id)
            ));
    }
}