using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Product;

namespace market_tracker_webapi.Application.Http.Models.Price;

public class PaginatedProductsOutputModel(
    PaginatedResult<ProductOffer> paginatedResult,
    ProductsHitsCounters hitsCounters
)
{
    public IEnumerable<ProductOffer> Items { get; } = paginatedResult.Items;
    public ProductsHitsCounters Hits { get; } = hitsCounters;
    public int CurrentPage { get; } = paginatedResult.CurrentPage;
    public int ItemsPerPage { get; } = paginatedResult.ItemsPerPage;
    public int TotalItems { get; } = paginatedResult.TotalItems;
    public int TotalPages { get; } = paginatedResult.TotalPages;
}

public class ProductsHitsCounters
{
    private Dictionary<int, HitsCounter> _brandsDictionary = new();
    private Dictionary<int, HitsCounter> _companiesDictionary = new();
    private Dictionary<int, HitsCounter> _categoriesDictionary = new();
    private BooleanHitsCounter _promotionDictionary = new();

    public IEnumerable<HitsCounter> Brands => _brandsDictionary.Values;
    public IEnumerable<HitsCounter> Companies => _companiesDictionary.Values;
    public IEnumerable<HitsCounter> Categories => _categoriesDictionary.Values;
    public BooleanHitsCounter Promotions => _promotionDictionary;

    public void AddOrUpdateBrandHitsCounter(int brandId, string brandName)
    {
        if (!_brandsDictionary.TryGetValue(brandId, out HitsCounter? value))
        {
            value = new HitsCounter
            {
                Id = brandId,
                Name = brandName
            };
            _brandsDictionary[brandId] = value;
        }

        value.Count++;
    }

    public void AddOrUpdateCompanyHitsCounter(int? companyId, string? companyName)
    {
        if (companyId == null || companyName == null)
        {
            return;
        }

        if (!_companiesDictionary.TryGetValue(companyId.Value, out HitsCounter? value))
        {
            value = new HitsCounter
            {
                Id = companyId.Value,
                Name = companyName
            };
            _companiesDictionary[companyId.Value] = value;
        }

        value.Count++;
    }

    public void AddOrUpdateCategoryHitsCounter(int categoryId, string categoryName)
    {
        if (!_categoriesDictionary.TryGetValue(categoryId, out HitsCounter? value))
        {
            value = new HitsCounter
            {
                Id = categoryId,
                Name = categoryName
            };
            _categoriesDictionary[categoryId] = value;
        }

        value.Count++;
    }

    public void AddOrUpdatePromotionHitsCounter(bool hasPromotion)
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

public class HitsCounter
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public int Count { get; set; } = 0;
}

public record BooleanHitsCounter
{
    public int False { get; set; } = 0;

    public int True { get; set; } = 0;
}