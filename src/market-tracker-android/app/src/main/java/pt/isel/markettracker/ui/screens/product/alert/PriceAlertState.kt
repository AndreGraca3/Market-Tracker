package pt.isel.markettracker.ui.screens.product.alert

import pt.isel.markettracker.domain.model.market.price.PriceAlert

sealed class PriceAlertState {
    data object Idle : PriceAlertState()
    data object Loading : PriceAlertState()

    sealed class Done : PriceAlertState()
    data class Created(val priceAlert: PriceAlert) : Done()
    data object Deleted : Done()
    data class Error(val error: Throwable) : Done()
}

fun PriceAlertState.extractPriceAlert(): PriceAlert? {
    return when (this) {
        is PriceAlertState.Created -> priceAlert
        else -> null
    }
}