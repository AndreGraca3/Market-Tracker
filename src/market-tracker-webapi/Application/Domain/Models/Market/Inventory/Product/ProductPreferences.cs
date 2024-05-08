namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

public record ProductPreferences(bool IsFavourite, ProductReview? Review);
