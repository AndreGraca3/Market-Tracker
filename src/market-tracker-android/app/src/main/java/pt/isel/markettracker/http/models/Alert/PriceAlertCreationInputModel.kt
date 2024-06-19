package pt.isel.markettracker.http.models.Alert

data class PriceAlertCreationInputModel(
    val productId: String,
    val storeId: Int,
    val priceThreshold: Int
)