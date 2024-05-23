using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Offer;

public record ProductsFiltersInputModel(
    [MaxLength(100)] string? Name,
    IList<int>? CategoryIds,
    IList<int>? BrandIds,
    IList<int>? CompanyIds,
    [Range(0, int.MaxValue)] int? MinPrice,
    [Range(0, int.MaxValue)] int? MaxPrice,
    [Range(1, 5)] int? MinRating,
    [Range(1, 5)] int? MaxRating,
    int MaxValuesPerFacet = 10
);