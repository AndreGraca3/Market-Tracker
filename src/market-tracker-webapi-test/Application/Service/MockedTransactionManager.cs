using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi_test.Application.Service;

public class MockedTransactionManager : ITransactionManager
{
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        return await action();
    }

    public async Task<Either<TError, T>> ExecuteAsync<TError, T>(
        Func<Task<Either<TError, T>>> action
    )
    {
        return await action();
    }
}
