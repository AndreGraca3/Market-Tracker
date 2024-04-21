using market_tracker_webapi.Application.Repository.Dto.Price;

namespace market_tracker_webapi.Application.Repository.Dto.Store;

public record StorePrice(StoreInfo Store, PriceInfo PriceData);
