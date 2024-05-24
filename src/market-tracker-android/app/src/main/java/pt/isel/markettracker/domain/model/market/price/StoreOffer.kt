package pt.isel.markettracker.domain.model.market.price

import pt.isel.markettracker.domain.model.market.Store
import pt.isel.markettracker.domain.model.market.StoreInfo
import java.time.LocalDateTime

data class StoreOffer(
    val store: Store,
    val priceData: Price,
    val isAvailable: Boolean,
    val lastChecked: LocalDateTime
)

data class StoreOfferItem(
    val store: StoreInfo,
    val priceData: Price,
    val isAvailable: Boolean,
    val lastChecked: LocalDateTime
)