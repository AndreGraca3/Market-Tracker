namespace market_tracker_webapi.Application.Http.Models;

public record ProductOutputModel(
    int Id,
    string Name,
    string Description,
    string ImageUrl,
    int Quantity,
    string Unit,
    int Views,
    float Rate,
    int BrandId,
    int CategoryId
);
