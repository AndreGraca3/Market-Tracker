namespace market_tracker_webapi.Application.Repository.Dto.Product;

public class PaginatedProductOffers(
    PaginatedResult<ProductOffer> paginatedResult,
    ProductsFacetsCounters facetsCounters)
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
    public ProductsFacetsCounters(
    )
    {
    }
    public ProductsFacetsCounters(IEnumerable<FacetCounter> brandFacets, IEnumerable<FacetCounter> companyFacets,
        IEnumerable<FacetCounter> categoryFacets,
        BooleanFacetsCounter promotionFacets)
    {
        Brands = brandFacets;
        Companies = companyFacets;
        Categories = categoryFacets;
        Promotions = promotionFacets;
    }

    private Dictionary<int, FacetCounter> _brandsDictionary = new();
    private Dictionary<int, FacetCounter> _companiesDictionary = new();
    private Dictionary<int, FacetCounter> _categoriesDictionary = new();
    private BooleanFacetsCounter _promotionDictionary = new();

    public IEnumerable<FacetCounter> Brands
    {
        get => _brandsDictionary.Values;
        set => throw new NotImplementedException();
    }

    public IEnumerable<FacetCounter> Companies
    {
        get => _companiesDictionary.Values;
        set => throw new NotImplementedException();
    }

    public IEnumerable<FacetCounter> Categories
    {
        get => _categoriesDictionary.Values;
        set => throw new NotImplementedException();
    }

    public BooleanFacetsCounter Promotions
    {
        get => _promotionDictionary;
        set => _promotionDictionary = value;
    }

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

    public int Count { get; set; }
}

public record BooleanFacetsCounter
{
    public int False { get; set; } = 0;

    public int True { get; set; } = 0;
}