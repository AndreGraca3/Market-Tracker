using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Repository.Dto.Product;

public record ProductOffer(ProductInfo Product, StorePrice? StorePrice);
