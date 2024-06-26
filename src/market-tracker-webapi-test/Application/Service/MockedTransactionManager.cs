﻿using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi_test.Application.Service;

public class MockedTransactionManager : ITransactionManager
{
    public Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        return action();
    }
}