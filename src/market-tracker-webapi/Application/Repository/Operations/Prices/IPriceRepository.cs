using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public interface IPriceRepository
{
    public Task<IEnumerable<PriceEntry>> GetCheapestPriceAtDateByProductIdAsync(
        int productId,
        DateTime date
    );

    public Task<IEnumerable<PriceEntry>> GetPricesByProductIdAsync();

    public Task AddPriceAsync(int productId, int price, DateTime date, int? promotionPercentage);
}
