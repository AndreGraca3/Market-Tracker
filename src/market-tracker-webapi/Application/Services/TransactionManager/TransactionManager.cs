using market_tracker_webapi.Infrastructure;

namespace market_tracker_webapi.Application.Services.TransactionManager
{
    public class TransactionManager
    {

        private static async Task<T?> TransactionHandler<T>(MarketTrackerDataContext marketTrackerDataContext, Func<Task<T>> action)
        {
            using var transaction = marketTrackerDataContext.Database.BeginTransaction();
            try
            {
                var result = await action();
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                // logger.LogError(ex.ToString());
                transaction.Rollback();
                throw;
            }
        }
    }
}
