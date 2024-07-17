package pt.isel.markettracker.ui.screens.favorites

import pt.isel.markettracker.domain.model.market.inventory.product.ProductItem

sealed class FavoriteScreenState {
    data object Idle : FavoriteScreenState()

    data object Loading : FavoriteScreenState()

    data class Loaded(
        val favorites: List<ProductItem>,
    ) : FavoriteScreenState()

    data class Failed(val error: Throwable) : FavoriteScreenState()
}

fun FavoriteScreenState.extractFavorites() =
    when (this) {
        is FavoriteScreenState.Loaded -> favorites
        else -> emptyList()
    }