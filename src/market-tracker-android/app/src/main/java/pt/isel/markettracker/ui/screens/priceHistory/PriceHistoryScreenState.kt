package pt.isel.markettracker.ui.screens.priceHistory

import pt.isel.markettracker.domain.model.market.inventory.product.PriceHistory

sealed class PriceHistoryScreenState {
    data object Idle: PriceHistoryScreenState()

    data object Loading: PriceHistoryScreenState()

    data class Loaded(
        val priceHistory: PriceHistory
    ): PriceHistoryScreenState()

    data class Failed(val error: Throwable) : PriceHistoryScreenState()
}