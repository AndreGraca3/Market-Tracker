package pt.isel.markettracker.domain.product

import java.time.LocalDateTime

data class ProductReview(
    val productId: String,
    val rating: Int,
    val text: String?,
    val clientId: String,
    val createdAt: LocalDateTime,
)
