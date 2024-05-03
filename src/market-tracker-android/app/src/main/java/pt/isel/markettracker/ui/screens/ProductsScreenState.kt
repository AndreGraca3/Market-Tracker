package pt.isel.markettracker.ui.screens

import pt.isel.markettracker.domain.model.product.Product
import pt.isel.markettracker.domain.model.product.ProductOffer

sealed class ProductsScreenState {
    data object Idle : ProductsScreenState()
    data object Loading : ProductsScreenState()
    data class Loaded(val query: ProductsQuery, val products: List<ProductOffer>) :
        ProductsScreenState()

    data class Error(val query: ProductsQuery, val error: Throwable) : ProductsScreenState()
}

fun ProductsScreenState.extractProducts() =
    when (this) {
        is ProductsScreenState.Loaded -> products
        else -> emptyList()
    }

data class ProductsQuery(
    val searchTerm: String? = null,
    val filters: ProductsFilters = ProductsFilters(),
    val sortOption: ProductsSortOption = ProductsSortOption.Popularity
)

data class ProductsFilters(
    val brandIds: List<Int>? = null,
    val categoryIds: List<Int>? = null,
    val minRating: String? = null,
    val maxRating: String? = null,
    val minPrice: String? = null,
    val maxPrice: String? = null
)

enum class ProductsSortOption(val title: String) {
    Popularity("Popularidade"),
    NameLowToHigh("Nome (A-Z)"),
    NameHighToLow("Nome (Z-A)"),
    RatingLowToHigh("Menor Avaliação"),
    RatingHighToLow("Maior Avaliação"),
    PriceLowToHigh("Menor Preço"),
    PriceHighToLow("Maior Preço")
}