package pt.isel.markettracker.domain.model.market.inventory.product

data class ProductPreferences(
    val isFavourite: Boolean,
    val review: ProductReview?
)
