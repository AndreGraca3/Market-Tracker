using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Offer;

public record PaginatedProductOffersOutputModel(
    IEnumerable<ProductOfferOutputModel> Items,
    ProductsFacetsCounters Facets,
    int CurrentPage,
    int ItemsPerPage,
    int TotalItems,
    int TotalPages
);

public static class PaginatedProductOffersOutputModelMapper
{
    public static PaginatedProductOffersOutputModel ToOutputModel(this PaginatedProductOffers paginatedProductOffers)
    {
        return new PaginatedProductOffersOutputModel(
            paginatedProductOffers.Items.Select(productOffer => productOffer.ToOutputModel()),
            paginatedProductOffers.Facets,
            paginatedProductOffers.CurrentPage,
            paginatedProductOffers.ItemsPerPage,
            paginatedProductOffers.TotalItems,
            paginatedProductOffers.TotalPages
        );
    }
}

public record ProductOfferOutputModel(
    ProductOutputModel Product,
    StoreOfferOutputModel? StoreOffer,
    bool IsAvailable
);

public static class ProductOfferOutputModelMapper
{
    public static ProductOfferOutputModel ToOutputModel(this ProductOffer productOffer)
    {
        return new ProductOfferOutputModel(
            productOffer.Product.ToOutputModel(),
            productOffer.StoreOffer?.ToOutputModel(),
            productOffer.IsAvailable
        );
    }
}