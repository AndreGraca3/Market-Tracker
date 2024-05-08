namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

public record ProductStats(string ProductId, ProductStatsCounts Counts, double AverageRating);

public record ProductStatsCounts(int Favourites, int Ratings);
