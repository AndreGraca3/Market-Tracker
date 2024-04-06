package pt.isel.markettracker.domain.product

data class ProductInfo(
    val id: Int,
    val name: String,
    val imageUrl: String,
    val lowestPriceStore: StorePriceData
)

data class StorePriceData(
    val storeId: Int,
    val storeName: String,
    val price: Double
)