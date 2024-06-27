using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Domain.Filters.Product;

using Product = Schemas.Market.Inventory.Product.Product;

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

public record PaginatedFacetedProducts(
    PaginatedResult<Product> PaginatedProducts,
    ProductsFacetsCounters FacetsCounters
);

public class ProductsFacetsCounters(
    IEnumerable<FacetCounter> brandFacets,
    IEnumerable<FacetCounter> categoryFacets,
    IEnumerable<FacetCounter> companyFacets)
{
    public IEnumerable<FacetCounter> Brands { get; set; } = brandFacets;
    public IEnumerable<FacetCounter> Categories { get; set; } = categoryFacets;
    public IEnumerable<FacetCounter> Companies { get; set; } = companyFacets;
}

public class FacetCounter
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public int Count { get; set; }
}