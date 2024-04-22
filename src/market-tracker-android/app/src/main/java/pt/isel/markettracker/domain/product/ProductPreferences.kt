package pt.isel.markettracker.domain.product

import pt.isel.markettracker.domain.price.PriceAlert

data class ProductPreferences(
    val isFavorite: Boolean,
    val priceAlert: PriceAlert?,
    val review: ProductReview?
)
