using market_tracker_webapi.Infrastructure;

namespace market_tracker_webapi.Application.Service.Transaction;

public class TransactionManager(MarketTrackerDataContext dataContext) : ITransactionManager
{
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        using var transaction = await dataContext.Database.BeginTransactionAsync();
        try
        {
            var result = await action();
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception ex)
        {
            // logger.LogError(ex.ToString());
            await transaction.RollbackAsync();
            throw;
        }
    }
}
