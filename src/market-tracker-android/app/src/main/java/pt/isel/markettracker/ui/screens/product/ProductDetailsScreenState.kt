package pt.isel.markettracker.ui.screens.product

import pt.isel.markettracker.domain.model.market.inventory.product.Product

sealed class ProductDetailsScreenState {
    data object Idle : ProductDetailsScreenState()
    data object LoadingProduct : ProductDetailsScreenState()
    data class LoadingProductDetails(val product: Product) : ProductDetailsScreenState()
    data class Loaded(val product: Product) : ProductDetailsScreenState()
    data class Failed(val error: Throwable) : ProductDetailsScreenState()
}

fun ProductDetailsScreenState.extractProduct() =
    when (this) {
        is ProductDetailsScreenState.Loaded -> product
        is ProductDetailsScreenState.LoadingProductDetails -> product
        else -> null
    }