package pt.isel.markettracker.domain.model.market.inventory.product

import pt.isel.markettracker.domain.model.account.ClientItem
import java.time.LocalDateTime

data class ProductReview(
    val id: Int,
    val productId: String,
    val rating: Int,
    val comment: String?,
    val createdAt: LocalDateTime,
    val client: ClientItem
)
