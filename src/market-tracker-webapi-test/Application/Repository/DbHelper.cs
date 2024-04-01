using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public static class DbHelper
{
    public static MarketTrackerDataContext CreateDatabase(params IEnumerable<object>[] entityCollections)
    {
        var options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new MarketTrackerDataContext(options);

        foreach (var entities in entityCollections)
        {
            foreach (var entity in entities)
            {
                databaseContext.Add(entity);
            }
        }

        databaseContext.SaveChanges();
        return databaseContext;
    }
}
