namespace market_tracker_webapi.Application.Http.Models;

public record ProductCreationInputModel(
    int Id,
    string Name,
    string Description,
    string ImageUrl,
    int Quantity,
    string Unit,
    int BrandId,
    int CategoryId
);
