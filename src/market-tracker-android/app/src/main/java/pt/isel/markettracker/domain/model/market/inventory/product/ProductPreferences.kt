package pt.isel.markettracker.domain.model.market.inventory.product

data class ProductPreferences(
    val isFavorite: Boolean,
    val review: ProductReview?
)
