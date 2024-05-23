package pt.isel.markettracker.ui.screens

import pt.isel.markettracker.domain.model.market.Company
import pt.isel.markettracker.domain.model.market.inventory.Brand
import pt.isel.markettracker.domain.model.market.inventory.Category
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.domain.model.market.inventory.product.ProductsFacetsCounters

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

    data class Error(val error: Throwable) :
        ProductsScreenState()
}

fun ProductsScreenState.extractProductsOffers() =
    when (this) {
        is ProductsScreenState.Loaded -> productsOffers
        else -> emptyList()
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