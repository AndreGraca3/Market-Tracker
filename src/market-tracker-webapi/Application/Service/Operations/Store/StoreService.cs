using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repositories.Store;

namespace market_tracker_webapi.Application.Services.Operations.Store
{
   public class StoreService(IStoreRepository storeRepository) : IStoreService
   {
      public Task<Domain.StoreDomain> GetStoreByIdAsync(int id)
      {
         throw new NotImplementedException();
      }

      public Task<int> AddStoreAsync(Domain.StoreDomain storeDomain)
      {
         throw new NotImplementedException();
      }

      public Task<Domain.StoreDomain> UpdateStoreAsync(Domain.StoreDomain storeDomain)
      {
         throw new NotImplementedException();
      }

      public Task<Domain.StoreDomain> DeleteStoreAsync(int id)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<Domain.StoreDomain>> GetStoresFromCompany(int id)
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

