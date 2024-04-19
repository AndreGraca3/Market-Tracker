using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Product;

public class ProductsFiltersInputModel
{
    public string? SearchName { get; set; }

    public IList<int>? CategoryIds { get; set; }

    public IList<int>? BrandIds { get; set; }

    public IList<int>? CompanyIds { get; set; }

    [Range(1, 5)] public int? MinRating { get; set; }

    [Range(1, 5)] public int? MaxRating { get; set; }
}