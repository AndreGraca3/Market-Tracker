package pt.isel.markettracker.ui.screens.products

import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer

sealed class ProductsScreenState {
    data object Idle : ProductsScreenState()

    data object Loading : ProductsScreenState()

    abstract class Loaded(
        open val productsOffers: List<ProductOffer>,
        open val hasMore: Boolean
    ) : ProductsScreenState()

    data class IdleLoaded(
        override val productsOffers: List<ProductOffer>,
        override val hasMore: Boolean
    ) : Loaded(productsOffers, hasMore)

    data class LoadingMore(
        override val productsOffers: List<ProductOffer>
    ) : Loaded(productsOffers, true)

    data class Failed(val error: Throwable) :
        ProductsScreenState()
}

fun ProductsScreenState.extractProductsOffers() =
    when (this) {
        is ProductsScreenState.Loaded -> productsOffers
        else -> emptyList()
    }

fun ProductsScreenState.extractHasMore() =
    when (this) {
        is ProductsScreenState.Loaded -> hasMore
        else -> false
    }

enum class ProductsSortOption(val title: String) {
    Popularity("Popularidade"),
    NameLowToHigh("Nome (A-Z)"),
    NameHighToLow("Nome (Z-A)"),
    RatingLowToHigh("Menor Avaliação"),
    RatingHighToLow("Maior Avaliação"),
    PriceLowToHigh("Menor Preço"),
    PriceHighToLow("Maior Preço");

    companion object {
        fun fromTitle(title: String) = entries.first { it.title == title }
    }
}