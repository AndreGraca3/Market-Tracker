using market_tracker_webapi.Application.Domain;

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
    BrandOutputModel Brand,
    Category Category
);
