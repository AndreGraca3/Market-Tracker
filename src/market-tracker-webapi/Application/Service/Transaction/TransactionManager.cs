using market_tracker_webapi.Application.Utils;
using market_tracker_webapi.Infrastructure;

namespace market_tracker_webapi.Application.Services.Transaction;

public class TransactionManager(MarketTrackerDataContext dataContext) : ITransactionManager
{
    public async Task<Either<TError, T>> ExecuteAsync<TError, T>(
        Func<Task<Either<TError, T>>> action
    )
    {
        using var transaction = await dataContext.Database.BeginTransactionAsync();
        try
        {
            var result = await action();
            if (!result.IsSuccessful())
            {
                await transaction.RollbackAsync();
                return result;
            }

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
