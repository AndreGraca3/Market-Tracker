package pt.isel.markettracker.domain.price

import pt.isel.markettracker.domain.City
import java.time.LocalDateTime

data class StorePrice(
    val storeId: Int,
    val storeName: String,
    val isOnline: Boolean,
    val city: City?,
    val price: Int,
    val promotion: Promotion?,
    val isAvailable: Boolean,
    val lastChecked: LocalDateTime
)