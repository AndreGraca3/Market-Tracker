package pt.isel.markettracker.domain.product

data class ProductStats(
    val productId: String,
    val counts: ProductStatsCounts,
    val averageRating: Double
)

data class ProductStatsCounts(
    val favourites: Int,
    val ratings: Int,
    val lists: Int
)