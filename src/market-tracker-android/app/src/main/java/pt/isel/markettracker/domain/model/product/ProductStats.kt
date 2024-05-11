package pt.isel.markettracker.domain.model.product

data class ProductStats(
    val productId: String,
    val counts: ProductStatsCounts,
    val averageRating: Double
)

data class ProductStatsCounts(
    val favorites: Int,
    val ratings: Int,
    val lists: Int
)