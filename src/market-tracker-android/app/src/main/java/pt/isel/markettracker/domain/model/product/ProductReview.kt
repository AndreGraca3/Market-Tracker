package pt.isel.markettracker.domain.model.product

import java.time.LocalDateTime

data class ProductReview(
    val productId: String,
    val rating: Int,
    val text: String?,
    val clientId: String,
    val createdAt: LocalDateTime,
)
