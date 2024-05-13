package pt.isel.markettracker.domain.model.market.inventory.product

import pt.isel.markettracker.domain.model.market.Company
import pt.isel.markettracker.domain.model.market.inventory.Brand
import pt.isel.markettracker.domain.model.market.inventory.Category
import pt.isel.markettracker.domain.model.market.price.StorePrice

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
    val brands: List<FacetCounter<Brand>>,
    val companies: List<FacetCounter<Company>>,
    val categories: List<FacetCounter<Category>>
)

data class FacetCounter<T>(
    val item: T,
    val count: Int
)