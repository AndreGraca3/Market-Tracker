using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository;

public static class DbHelper
{
    public static MarketTrackerDataContext CreateDatabase<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        var options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        databaseContext.Set<TEntity>().AddRange(entities);

        databaseContext.SaveChanges();
        return databaseContext;
    }
}