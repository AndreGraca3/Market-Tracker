package pt.isel.markettracker.domain.model.market.price

import java.time.LocalDateTime

data class PriceAlert(
    val id: String,
    val product: ProductAlert,
    val store: StoreItem,
    val priceThreshold: Int,
    val createdAt: LocalDateTime,
)

data class ProductAlert(
    val id: String,
    val name: String,
    val imageUrl: String,
)

data class PriceAlertId(
    val value: String,
)

data class StoreItem(
    val id: Int,
    val name: String,
)