using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public interface IPriceRepository
{
    public Task<IEnumerable<PriceEntry>> GetPricesAsync();

    public Task AddPriceAsync(int productId, int price, DateTime date, int? promotionPercentage);
}
