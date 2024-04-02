using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repository.Operations.City;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Repository.Operations.Store;
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
        public async Task<CollectionOutputModel> GetStoresAsync()
        {
            var stores = await storeRepository.GetStoresAsync();
            return new CollectionOutputModel(stores);
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
            Either<StoreFetchingError, IEnumerable<Domain.Store>>
        > GetStoresFromCompanyAsync(int companyId)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var company = await companyRepository.GetCompanyByIdAsync(companyId);
                if (company is null)
                {
                    return EitherExtensions.Failure<StoreFetchingError, IEnumerable<Domain.Store>>(
                        new StoreFetchingError.StoreByCompanyIdNotFound(companyId)
                    );
                }

                var stores = await storeRepository.GetStoresFromCompanyAsync(companyId);
                return EitherExtensions.Success<StoreFetchingError, IEnumerable<Domain.Store>>(
                    stores
                );
            });
        }

        public async Task<
            Either<StoreFetchingError, IEnumerable<Domain.Store>>
        > GetStoresByCityNameAsync(string cityName)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var city = await cityRepository.GetCityByNameAsync(cityName);
                if (city is null)
                {
                    return EitherExtensions.Failure<StoreFetchingError, IEnumerable<Domain.Store>>(
                        new StoreFetchingError.StoreByCityNameNotFound(cityName)
                    );
                }

                var stores = await storeRepository.GetStoresByCityNameAsync(cityName);
                return EitherExtensions.Success<StoreFetchingError, IEnumerable<Domain.Store>>(
                    stores
                );
            });
        }

        public async Task<Either<IStoreError, IdOutputModel>> AddStoreAsync(
            string name,
            string address,
            int cityId,
            int companyId
        )
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                if (await storeRepository.GetStoreByAddressAsync(address) is not null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreCreationError.StoreAddressAlreadyExists(address)
                    );
                }

                if (await storeRepository.GetStoreByNameAsync(name) is not null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreCreationError.StoreNameAlreadyExists(name)
                    );
                }

                if (await cityRepository.GetCityByIdAsync(cityId) is null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreFetchingError.StoreByCityIdNotFound(cityId)
                    );
                }

                if (await companyRepository.GetCompanyByIdAsync(companyId) is null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreFetchingError.StoreByCompanyIdNotFound(companyId)
                    );
                }

                var storeId = await storeRepository.AddStoreAsync(name, address, cityId, companyId);
                return EitherExtensions.Success<IStoreError, IdOutputModel>(
                    new IdOutputModel(storeId)
                );
            });
        }

        public async Task<Either<IStoreError, IdOutputModel>> UpdateStoreAsync(
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
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreFetchingError.StoreByIdNotFound(id)
                    );
                }

                if (await storeRepository.GetStoreByNameAsync(name) is not null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreCreationError.StoreNameAlreadyExists(name)
                    );
                }

                if (await storeRepository.GetStoreByAddressAsync(address) is not null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreCreationError.StoreAddressAlreadyExists(address)
                    );
                }

                if (await cityRepository.GetCityByIdAsync(cityId) is null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreFetchingError.StoreByCityIdNotFound(cityId)
                    );
                }

                if (await companyRepository.GetCompanyByIdAsync(companyId) is null)
                {
                    return EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreFetchingError.StoreByCompanyIdNotFound(companyId)
                    );
                }

                var updatedStore = await storeRepository.UpdateStoreAsync(
                    id,
                    address,
                    cityId,
                    companyId
                );
                return updatedStore is null
                    ? EitherExtensions.Failure<IStoreError, IdOutputModel>(
                        new StoreFetchingError.StoreByIdNotFound(id)
                    )
                    : EitherExtensions.Success<IStoreError, IdOutputModel>(
                        new IdOutputModel(updatedStore.Id)
                    );
            });
        }

        public async Task<Either<StoreFetchingError, IdOutputModel>> DeleteStoreAsync(int id)
        {
            var store = await storeRepository.GetStoreByIdAsync(id);
            if (store is null)
            {
                return EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(id)
                );
            }

            await storeRepository.DeleteStoreAsync(id);
            return EitherExtensions.Success<StoreFetchingError, IdOutputModel>(
                new IdOutputModel(id)
            );
        }
    }
}
