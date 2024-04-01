using market_tracker_webapi.Application.Services.Transaction;

namespace market_tracker_webapi_test.Application.Services;

public class MockedTransactionManager: ITransactionManager
{
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        return await action();
    }
}