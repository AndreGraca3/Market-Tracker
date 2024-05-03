package pt.isel.markettracker.domain.model.product

import pt.isel.markettracker.domain.model.price.StorePrice

data class ProductOffer(
    val product: Product,
    val storePrice: StorePrice
)

data class PaginatedProductOffers(
    val items: List<ProductOffer>,
    val facets: ProductsFacetsCounters,
    val currentPage: Int,
    val itemsPerPage: Int,
    val totalItems: Int,
    val totalPages: Int,
    val hasMore: Boolean = currentPage < totalPages
)

data class ProductsFacetsCounters(
    val brands: List<FacetCounter>,
    val categories: List<FacetCounter>,
    val companies: List<FacetCounter>
)

data class FacetCounter(
    val id: Int,
    val name: String,
    val count: Int
)