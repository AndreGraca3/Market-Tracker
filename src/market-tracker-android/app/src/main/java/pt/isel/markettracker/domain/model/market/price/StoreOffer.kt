package pt.isel.markettracker.domain.model.market.price

import pt.isel.markettracker.domain.model.market.Company
import pt.isel.markettracker.domain.model.market.Store
import pt.isel.markettracker.domain.model.market.StoreInfo
import java.time.LocalDateTime

data class StoreOffer(
    val store: Store,
    val price: Price,
    val isAvailable: Boolean,
    val lastChecked: LocalDateTime
) {
    fun toStoreOfferItem() = StoreOfferItem(
        store.toStoreInfo(),
        price,
        isAvailable,
        lastChecked
    )
}

data class StoreOfferItem(
    val store: StoreInfo,
    val price: Price,
    val isAvailable: Boolean,
    val lastChecked: LocalDateTime
) {
    fun toStoreOffer(company: Company) = StoreOffer(
        store.toStore(company),
        price,
        isAvailable,
        lastChecked
    )
}