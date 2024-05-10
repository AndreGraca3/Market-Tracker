using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Store
{
    public class StoreService(
        IStoreRepository storeRepository,
        ICityRepository cityRepository,
        ICompanyRepository companyRepository,
        ITransactionManager transactionManager
    ) : IStoreService
    {
        public async Task<Either<IServiceError, CollectionOutputModel<Domain.Models.Market.Retail.Shop.Store>>> GetStoresAsync(
            int? companyId = null,
            int? cityId = null,
            string? name = null
            )
        {
            var stores = await storeRepository.GetStoresAsync(companyId, cityId, name);
            return EitherExtensions.Success<IServiceError, CollectionOutputModel<Domain.Models.Market.Retail.Shop.Store>>(
                new CollectionOutputModel<Domain.Models.Market.Retail.Shop.Store>(stores)
            );
        }

        public async Task<Either<StoreFetchingError, Domain.Models.Market.Retail.Shop.Store>> GetStoreByIdAsync(int id)
        {
            var store = await storeRepository.GetStoreByIdAsync(id);
            return store is null
                ? EitherExtensions.Failure<StoreFetchingError, Domain.Models.Market.Retail.Shop.Store>(
                    new StoreFetchingError.StoreByIdNotFound(id)
                )
                : EitherExtensions.Success<StoreFetchingError, Domain.Models.Market.Retail.Shop.Store>(store);
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