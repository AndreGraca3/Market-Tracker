package pt.isel.markettracker.domain.model.market.price

import java.time.LocalDateTime

data class Price(
    val regularPrice: Int,
    val finalPrice: Int,
    val promotion: Promotion?,
    val createdAt: LocalDateTime
)

data class Promotion(
    val percentage: Int,
    val createdAt: LocalDateTime
)