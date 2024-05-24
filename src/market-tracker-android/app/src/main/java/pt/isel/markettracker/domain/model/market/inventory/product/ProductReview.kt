package pt.isel.markettracker.domain.model.market.inventory.product

import pt.isel.markettracker.domain.model.account.ClientItem
import java.time.LocalDateTime

data class ProductReview(
    val productId: String,
    val rating: Int,
    val text: String?,
    val client: ClientItem,
    val createdAt: LocalDateTime,
)
