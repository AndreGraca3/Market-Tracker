package pt.isel.markettracker.domain.model.market.price

import pt.isel.markettracker.domain.model.market.Store
import java.time.LocalDateTime

data class StorePrice(
    val store: Store,
    val priceData: Price,
    val isAvailable: Boolean,
    val lastChecked: LocalDateTime
)