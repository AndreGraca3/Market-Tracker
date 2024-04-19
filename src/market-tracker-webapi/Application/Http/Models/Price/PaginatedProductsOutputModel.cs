using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Product;

namespace market_tracker_webapi.Application.Http.Models.Price;

public class PaginatedProductsOutputModel(
    PaginatedResult<ProductOffer> paginatedResult,
    ProductsFacetsCounters facetsCounters
)
{
    public IEnumerable<ProductOffer> Items { get; } = paginatedResult.Items;
    public ProductsFacetsCounters Facets { get; } = facetsCounters;
    public int CurrentPage { get; } = paginatedResult.CurrentPage;
    public int ItemsPerPage { get; } = paginatedResult.ItemsPerPage;
    public int TotalItems { get; } = paginatedResult.TotalItems;
    public int TotalPages { get; } = paginatedResult.TotalPages;
}

public class ProductsFacetsCounters
{
    private Dictionary<int, FacetCounter> _brandsDictionary = new();
    private Dictionary<int, FacetCounter> _companiesDictionary = new();
    private Dictionary<int, FacetCounter> _categoriesDictionary = new();
    private BooleanFacetsCounter _promotionDictionary = new();

    public IEnumerable<FacetCounter> Brands => _brandsDictionary.Values;
    public IEnumerable<FacetCounter> Companies => _companiesDictionary.Values;
    public IEnumerable<FacetCounter> Categories => _categoriesDictionary.Values;
    public BooleanFacetsCounter Promotions => _promotionDictionary;

    public void AddOrUpdateBrandFacetCounter(int brandId, string brandName)
    {
        if (!_brandsDictionary.TryGetValue(brandId, out FacetCounter? value))
        {
            value = new FacetCounter
            {
                Id = brandId,
                Name = brandName
            };
            _brandsDictionary[brandId] = value;
        }

        value.Count++;
    }

    public void AddOrUpdateCompanyFacetCounter(int? companyId, string? companyName)
    {
        if (companyId == null || companyName == null)
        {
            return;
        }

        if (!_companiesDictionary.TryGetValue(companyId.Value, out FacetCounter? value))
        {
            value = new FacetCounter
            {
                Id = companyId.Value,
                Name = companyName
            };
            _companiesDictionary[companyId.Value] = value;
        }

        value.Count++;
    }

    public void AddOrUpdateCategoryFacetCounter(int categoryId, string categoryName)
    {
        if (!_categoriesDictionary.TryGetValue(categoryId, out FacetCounter? value))
        {
            value = new FacetCounter
            {
                Id = categoryId,
                Name = categoryName
            };
            _categoriesDictionary[categoryId] = value;
        }

        value.Count++;
    }

    public void AddOrUpdatePromotionFacetCounter(bool hasPromotion)
    {
        if (hasPromotion)
        {
            _promotionDictionary.True++;
        }
        else
        {
            _promotionDictionary.False++;
        }
    }
}

public class FacetCounter
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public int Count { get; set; } = 0;
}

public record BooleanFacetsCounter
{
    public int False { get; set; } = 0;

    public int True { get; set; } = 0;
}