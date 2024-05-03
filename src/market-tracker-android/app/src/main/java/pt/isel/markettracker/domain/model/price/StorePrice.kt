package pt.isel.markettracker.domain.model.price

import pt.isel.markettracker.domain.model.city.City
import pt.isel.markettracker.domain.model.product.StoreInfo
import java.time.LocalDateTime

data class StorePrice(
    val id: Int,
    val name: String,
    val address: String,
    val isOnline: Boolean,
    val city: City?,
    val price: Int,
    val promotion: Promotion?,
    val isAvailable: Boolean,
    val lastChecked: LocalDateTime
) {
    constructor(
        store: StoreInfo,
        price: Int,
        promotion: Promotion?,
        isAvailable: Boolean,
        lastChecked: LocalDateTime
    ) : this(
        store.id,
        store.name,
        store.address,
        store.isOnline,
        store.city,
        price,
        promotion,
        isAvailable,
        lastChecked
    )
}