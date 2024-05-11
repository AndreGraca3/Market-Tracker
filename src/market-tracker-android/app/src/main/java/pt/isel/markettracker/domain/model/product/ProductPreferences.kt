package pt.isel.markettracker.domain.model.product

import pt.isel.markettracker.domain.model.price.PriceAlert

data class ProductPreferences(
    val isFavorite: Boolean,
    val priceAlert: PriceAlert?,
    val review: ProductReview?
)
