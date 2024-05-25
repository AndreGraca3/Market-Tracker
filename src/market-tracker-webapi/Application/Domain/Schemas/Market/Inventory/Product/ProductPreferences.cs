namespace market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

public record ProductPreferences(bool IsFavourite, ProductReview? Review);
