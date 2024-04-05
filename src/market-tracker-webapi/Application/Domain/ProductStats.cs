namespace market_tracker_webapi.Application.Domain;

public record ProductStats(int ProductId, ProductStatsCounts Counts, double AverageRating);

public record ProductStatsCounts(int Favourites, int Ratings, int Lists);
