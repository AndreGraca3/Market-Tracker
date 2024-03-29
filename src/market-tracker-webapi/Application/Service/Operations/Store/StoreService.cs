﻿using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.City;
using market_tracker_webapi.Application.Repositories.Company;
using market_tracker_webapi.Application.Repositories.Store;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Services.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Store
{
   public class StoreService(
      IStoreRepository storeRepository,
      ICityRepository cityRepository,
      ICompanyRepository companyRepository,
      ITransactionManager transactionManager) : IStoreService
   {
      public Task<IEnumerable<StoreDomain>> GetStoresAsync()
      {
         var stores = storeRepository.GetStoresAsync();
         return stores;
      }

      public async Task<Either<StoreFetchingError, StoreDomain>> GetStoreByIdAsync(int id)
      {
         var store = await storeRepository.GetStoreByIdAsync(id);
         return store is null
            ? EitherExtensions.Failure<StoreFetchingError, StoreDomain>(
               new StoreFetchingError.StoreByIdNotFound(id)
            )
            : EitherExtensions.Success<StoreFetchingError, StoreDomain>(store);
      }

      public async Task<Either<IStoreError, IdOutputModel>> AddStoreAsync(string address, int cityId, int companyId)
      {
         return await transactionManager.ExecuteAsync(async () =>
         {
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

            var storeId = await storeRepository.AddStoreAsync(address, cityId, companyId);
            return EitherExtensions.Success<IStoreError, IdOutputModel>(
               new IdOutputModel
               {
                  Id = storeId
               }
            );
         });
      }

      public async Task<Either<StoreFetchingError, IdOutputModel>> UpdateStoreAsync(int id, string address, int cityId, int companyId)
      {
         return await transactionManager.ExecuteAsync(async () =>
         {
            var store = await storeRepository.GetStoreByIdAsync(id);
            if (store is null)
            {
               return EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                  new StoreFetchingError.StoreByIdNotFound(id)
               );
            }
   
            if (await cityRepository.GetCityByIdAsync(cityId) is null)
            {
               return EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                  new StoreFetchingError.StoreByCityIdNotFound(cityId)
               );
            }
   
            if (await companyRepository.GetCompanyByIdAsync(companyId) is null)
            {
               return EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                  new StoreFetchingError.StoreByCompanyIdNotFound(companyId)
               );
            }
   
            var updatedStore = await storeRepository.UpdateStoreAsync(id, address, cityId, companyId);
            return updatedStore is null
               ? EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                  new StoreFetchingError.StoreByIdNotFound(id)
               )
               : EitherExtensions.Success<StoreFetchingError, IdOutputModel>(
                  new IdOutputModel
                  {
                     Id = updatedStore.Id
                  }
               );
         });
         
      }

      public async Task<Either<StoreFetchingError, IdOutputModel>> DeleteStoreAsync(int id)
      {
         var store = await storeRepository.GetStoreByIdAsync(id);
         if (store is null)
         {
            return EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
               new StoreFetchingError.StoreByIdNotFound(id));
         }
         
         await storeRepository.DeleteStoreAsync(id);
         return EitherExtensions.Success<StoreFetchingError, IdOutputModel>(
            new IdOutputModel
            {
               Id = id
            }
         );
      }

      public Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>> GetStoresFromCompany(int companyId)
      {
         throw new NotImplementedException();
      }

      public Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>> GetStoresFromCityByName(string cityName)
      {
         throw new NotImplementedException();
      }
   } 
}

