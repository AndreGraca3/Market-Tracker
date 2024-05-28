package pt.isel.markettracker.domain.model.market.inventory.product

import pt.isel.markettracker.domain.model.market.inventory.product.filter.FacetItem
import pt.isel.markettracker.domain.model.market.price.StoreOffer

data class ProductOffer(
    val product: Product,
    val storeOffer: StoreOffer
)

data class PaginatedProductOffers(
    val items: List<ProductOffer>,
    val facets: ProductsFacetsCounters,
    val currentPage: Int,
    val itemsPerPage: Int,
    val totalItems: Int,
    val totalPages: Int
) {
    val hasMore: Boolean
        get() = currentPage < totalPages
}

data class ProductsFacetsCounters(
    val brands: List<FacetItem<Int>>,
    val companies: List<FacetItem<Int>>,
    val categories: List<FacetItem<Int>>
)