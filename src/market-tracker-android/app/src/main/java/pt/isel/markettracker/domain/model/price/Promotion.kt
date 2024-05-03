package pt.isel.markettracker.domain.model.price

data class Promotion(
    val id: String,
    val percentage: Int,
    val oldPrice: Int,
    val createdAt: String
)
