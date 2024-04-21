namespace market_tracker_webapi.Application.Domain;

public record Product(
    string Id,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    int Views,
    double Rating,
    int BrandId,
    int CategoryId
);
