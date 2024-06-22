package pt.isel.markettracker.ui.screens.product.rating

import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences

sealed class ProductPreferencesState {
    data object Idle : ProductPreferencesState()
    data object Loading : ProductPreferencesState()

    abstract class Done : ProductPreferencesState()
    data class Loaded(val preferences: ProductPreferences) : Done()
    data object Unauthenticated : Done()
    data class Failed(val error: Throwable) : Done()
}

fun ProductPreferencesState.extractPreferences(): ProductPreferences? =
    when(this) {
        is ProductPreferencesState.Loaded -> preferences
        else -> null
    }