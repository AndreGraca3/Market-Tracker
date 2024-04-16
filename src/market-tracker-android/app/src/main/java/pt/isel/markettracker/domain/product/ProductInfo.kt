package pt.isel.markettracker.domain.product

data class ProductInfo(
    val id: String,
    val name: String,
    val imageUrl: String,
    val brand: String,
    val category: String,
    val lowestPriceStore: StorePriceData,
)

data class StorePriceData(
    val storeId: Int,
    val storeName: String,
    val company: CompanyInfo,
    val price: Int
)

data class CompanyInfo(
    val id: Int,
    val name: String,
    val logoUrl: String
)