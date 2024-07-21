package pt.isel.markettracker.domain.model.market.inventory.product

import java.time.LocalDateTime

data class PriceHistory(
    val productId: String,
    val history: List<ProductPriceHistoryEntry>,
    val numberOfListPresent: Int,
)

data class ProductPriceHistoryEntry(
    val date: LocalDateTime,
    val price: Int,
)