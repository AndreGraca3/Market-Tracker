using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi_test.Application.Services;

public class MockedTransactionManager: ITransactionManager
{
    public async Task<Either<TError, T>> ExecuteAsync<TError, T>(Func<Task<Either<TError, T>>> action)
    {
        return await action();
    }
}