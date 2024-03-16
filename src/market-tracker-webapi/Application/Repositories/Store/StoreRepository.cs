using market_tracker_webapi.Application.Exceptions;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repositories.Store
{
    public class StoreRepository(MarketTrackerDataContext marketTrackerDataContext) : IStoreRepository
    {
        public async Task<StoreData?> GetStoreByIdAsync(int id)
        {
            var storeEntity = await marketTrackerDataContext.Store.FindAsync(id);
            return storeEntity != null ? MapStoreEntity(storeEntity) : null;
        }

        public async Task<int> AddStoreAsync(StoreData storeData)
        {
            var company = await marketTrackerDataContext.Company.FindAsync(storeData.CompanyId); //Change to Query instead of LINQ

            if (company == null)
            {
                throw new EntityNotFoundException($"Company with Id {storeData.CompanyId} not found.");
            }
            
            var newStore = new StoreEntity
            {
                Address = storeData.Address,
                City = storeData.City,
                OpenTime = storeData.OpenTime,
                CloseTime = storeData.CloseTime,
                CompanyId = storeData.CompanyId
            };

            try
            {
                marketTrackerDataContext.Store.Add(newStore);
                await marketTrackerDataContext.SaveChangesAsync();
    
                return newStore.Id;
            } 
            catch (DbUpdateException e) when (e.GetBaseException() is SqlException { Number: ErrorCodes.CannotInsertDuplicateKeySqlErrorCode })
            {
                throw new EntityCreationException("Duplicate values were found in the table MarketTracker.store");
            }   
        }

        public Task<StoreData?> UpdateStoreAsync(int id, DateTime? openTime, DateTime? closeTime)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStoreAsync(int id)
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

        private static StoreData MapStoreEntity(StoreEntity storeEntity)
        {
            return new StoreData
            {
                Address = storeEntity.Address,
                City = storeEntity.City,
                OpenTime = storeEntity.OpenTime,
                CloseTime = storeEntity.CloseTime,
                CompanyId = storeEntity.CompanyId
            };
        }
    }
}