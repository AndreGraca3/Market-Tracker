package pt.isel.markettracker.domain.model

import java.time.LocalDateTime

data class PriceAlertOutputModel(
    val id: String,
    val productId: String,
    val storeId: Int,
    val priceThreshold: Float,
    val createdAt: LocalDateTime
)