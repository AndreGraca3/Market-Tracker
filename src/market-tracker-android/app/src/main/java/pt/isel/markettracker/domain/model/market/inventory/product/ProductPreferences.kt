package pt.isel.markettracker.domain.model.market.inventory.product

import pt.isel.markettracker.domain.model.market.price.PriceAlert

data class ProductPreferences(
    val isFavorite: Boolean,
    val priceAlert: PriceAlert?,
    val review: ProductReview?
)
