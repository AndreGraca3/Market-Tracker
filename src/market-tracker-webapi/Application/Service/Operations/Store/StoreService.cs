using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repositories.Store;

namespace market_tracker_webapi.Application.Service.Operations.Store
{
   public class StoreService(IStoreRepository storeRepository) : IStoreService
   {
      public Task<StoreDomain> GetStoreByIdAsync(int id)
      {
         throw new NotImplementedException();
      }

      public Task<int> AddStoreAsync(StoreDomain storeDomain)
      {
         throw new NotImplementedException();
      }

      public Task<StoreDomain> UpdateStoreAsync(StoreDomain storeDomain)
      {
         throw new NotImplementedException();
      }

      public Task<Domain.StoreDomain> DeleteStoreAsync(int id)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<StoreDomain>> GetStoresFromCompany(int id)
      {
         throw new NotImplementedException();
      }

      public Task<CompanyDomain> GetCompanyAsync(int id)
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

