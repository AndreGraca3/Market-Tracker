using market_tracker_webapi.Infrastructure;

namespace market_tracker_webapi.Application.Repository.Transaction
{
    public class TransactionHandler
    {
        private static async Task<T?> TransactionHandler<T>(Func<Task<T>> action)
        {
            using var transaction = _marketTrackerDataContext.Database.BeginTransaction();
            try
            {
                var result = await action();
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _logger.BuildLogError(ex.ToString());
                transaction.Rollback();
                throw;
            }
        }
    }
}
