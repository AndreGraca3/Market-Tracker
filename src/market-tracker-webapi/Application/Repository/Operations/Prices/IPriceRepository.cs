using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public interface IPriceRepository
{
    public Task<IEnumerable<PriceEntry>> GetCheapestPriceAtDateByProductIdAsync(
        int productId,
        DateTime createdAt
    );

    public Task<IEnumerable<PriceEntry>> GetPricesByProductIdAsync(int productId);

    public Task AddPriceAsync(
        int productId,
        int price,
        DateTime createdAt,
        int? promotionPercentage
    );
}
