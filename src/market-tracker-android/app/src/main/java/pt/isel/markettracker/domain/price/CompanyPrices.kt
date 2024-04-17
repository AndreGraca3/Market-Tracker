package pt.isel.markettracker.domain.price

data class CompanyPrices(
    val id: Int,
    val name: String,
    val stores: List<StorePrice>
)