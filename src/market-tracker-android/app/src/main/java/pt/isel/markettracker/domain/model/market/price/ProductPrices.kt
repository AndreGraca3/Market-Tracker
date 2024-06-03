package pt.isel.markettracker.domain.model.market.price

data class ProductPrices(
    val companies: List<CompanyPrices>,
    val minPrice: Int,
    val maxPrice: Int
)

data class CompanyPrices(
    val id: Int,
    val name: String,
    val logoUrl: String,
    val stores: List<StoreOfferItem>
)