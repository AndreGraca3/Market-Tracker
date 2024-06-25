package pt.isel.markettracker.domain.model.market.price

import java.time.LocalDateTime

data class PriceAlert(
    val id: String,
    val productId: String,
    val storeId: Int,
    val priceThreshold: Int,
    val createdAt: LocalDateTime
)

data class PriceAlertId(
    val id: String
)