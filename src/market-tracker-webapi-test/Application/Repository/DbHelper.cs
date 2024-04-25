using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository;

public static class DbHelper
{
    public static MarketTrackerDataContext CreateDatabase(params IEnumerable<object>[] entityCollections)
    {
        var options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
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