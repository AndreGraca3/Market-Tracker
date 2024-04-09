using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.City;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Repository.Operations.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Store
{
    public class StoreService(
        IStoreRepository storeRepository,
        ICityRepository cityRepository,
        ICompanyRepository companyRepository,
        ITransactionManager transactionManager
    ) : IStoreService
    {
        public async Task<Either<IServiceError, CollectionOutputModel>> GetStoresAsync()
        {
            var stores = await storeRepository.GetStoresAsync();
            return EitherExtensions.Success<IServiceError, CollectionOutputModel>(
                new CollectionOutputModel(stores)
            );
        }

        public async Task<Either<StoreFetchingError, Domain.Store>> GetStoreByIdAsync(int id)
        {
            var store = await storeRepository.GetStoreByIdAsync(id);
            return store is null
                ? EitherExtensions.Failure<StoreFetchingError, Domain.Store>(
                    new StoreFetchingError.StoreByIdNotFound(id)
                )
                : EitherExtensions.Success<StoreFetchingError, Domain.Store>(store);
        }

        public async Task<
            Either<CompanyFetchingError, IEnumerable<Domain.Store>>
        > GetStoresFromCompanyAsync(int companyId)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var company = await companyRepository.GetCompanyByIdAsync(companyId);
                if (company is null)
                {
                    return EitherExtensions.Failure<CompanyFetchingError, IEnumerable<Domain.Store>>(
                        new CompanyFetchingError.CompanyByIdNotFound(companyId)
                    );
                }

                var stores = await storeRepository.GetStoresFromCompanyAsync(companyId);
                return EitherExtensions.Success<CompanyFetchingError, IEnumerable<Domain.Store>>(
                    stores
                );
            });
        }

        public async Task<
            Either<CityFetchingError, IEnumerable<Domain.Store>>
        > GetStoresByCityNameAsync(string cityName)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var city = await cityRepository.GetCityByNameAsync(cityName);
                if (city is null)
                {
                    return EitherExtensions.Failure<CityFetchingError, IEnumerable<Domain.Store>>(
                        new CityFetchingError.CityByNameNotFound(cityName)
                    );
                }

                var stores = await storeRepository.GetStoresByCityNameAsync(cityName);
                return EitherExtensions.Success<CityFetchingError, IEnumerable<Domain.Store>>(
                    stores
                );
            });
        }

        public async Task<Either<IServiceError, IntIdOutputModel>> AddStoreAsync(
            string name,
            string address,
            int? cityId,
            int companyId
        )
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                if (await storeRepository.GetStoreByAddressAsync(address) is not null)
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new StoreCreationError.StoreAddressAlreadyExists(address)
                    );
                }

                if (await storeRepository.GetStoreByNameAsync(name) is not null)
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new StoreCreationError.StoreNameAlreadyExists(name)
                    );
                }

                if (
                    cityId.HasValue
                    && await cityRepository.GetCityByIdAsync(cityId.Value) is null
                )
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new CityFetchingError.CityByIdNotFound(cityId.Value)
                    );
                }

                if (await companyRepository.GetCompanyByIdAsync(companyId) is null)
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new CompanyFetchingError.CompanyByIdNotFound(companyId)
                    );
                }

                var storeId = await storeRepository.AddStoreAsync(name, address, cityId, companyId);
                return EitherExtensions.Success<IServiceError, IntIdOutputModel>(
                    new IntIdOutputModel(storeId)
                );
            });
        }

        public async Task<Either<IServiceError, IntIdOutputModel>> UpdateStoreAsync(
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
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new StoreFetchingError.StoreByIdNotFound(id)
                    );
                }

                if (await storeRepository.GetStoreByNameAsync(name) is not null)
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new StoreCreationError.StoreNameAlreadyExists(name)
                    );
                }

                if (await storeRepository.GetStoreByAddressAsync(address) is not null)
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new StoreCreationError.StoreAddressAlreadyExists(address)
                    );
                }

                if (await cityRepository.GetCityByIdAsync(cityId) is null)
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new CityFetchingError.CityByIdNotFound(cityId)
                    );
                }

                if (await companyRepository.GetCompanyByIdAsync(companyId) is null)
                {
                    return EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new CompanyFetchingError.CompanyByIdNotFound(companyId)
                    );
                }

                var updatedStore = await storeRepository.UpdateStoreAsync(
                    id,
                    address,
                    cityId,
                    companyId
                );
                return updatedStore is null
                    ? EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                        new StoreFetchingError.StoreByIdNotFound(id)
                    )
                    : EitherExtensions.Success<IServiceError, IntIdOutputModel>(
                        new IntIdOutputModel(updatedStore.Id)
                    );
            });
        }

        public async Task<Either<StoreFetchingError, IntIdOutputModel>> DeleteStoreAsync(int id)
        {
            var store = await storeRepository.GetStoreByIdAsync(id);
            if (store is null)
            {
                return EitherExtensions.Failure<StoreFetchingError, IntIdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(id)
                );
            }

            await storeRepository.DeleteStoreAsync(id);
            return EitherExtensions.Success<StoreFetchingError, IntIdOutputModel>(
                new IntIdOutputModel(id)
            );
        }
    }
}
