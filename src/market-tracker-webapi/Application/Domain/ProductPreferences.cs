namespace market_tracker_webapi.Application.Domain;

public record ProductPreferences(bool IsFavourite, ProductReview? Review);
