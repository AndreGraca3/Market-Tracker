using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Models.Company;
using market_tracker_webapi.Application.Repositories.Store;

namespace market_tracker_webapi.Application.Services.Store
{
   public class StoreService(IStoreRepository storeRepository) : IStoreService
   {
      public Task<StoreData> GetStoreByIdAsync(int id)
      {
         throw new NotImplementedException();
      }

      public Task<int> AddStoreAsync(StoreData storeData)
      {
         throw new NotImplementedException();
      }

      public Task<StoreData> UpdateStoreAsync(StoreData storeData)
      {
         throw new NotImplementedException();
      }

      public Task<StoreData> DeleteStoreAsync(int id)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<StoreData>> GetStoresFromCompany(int id)
      {
         throw new NotImplementedException();
      }

      public Task<CompanyData> GetCompanyAsync(int id)
      {
         throw new NotImplementedException();
      }

      public Task<int> AddCompanyAsync(string companyName)
      {
         throw new NotImplementedException();
      }

      public Task<int> DeleteCompanyAsync(int id)
      {
         throw new NotImplementedException();
      }
   } 
}

