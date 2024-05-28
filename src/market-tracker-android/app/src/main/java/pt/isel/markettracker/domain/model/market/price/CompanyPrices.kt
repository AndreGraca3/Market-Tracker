package pt.isel.markettracker.domain.model.market.price

data class CompanyPrices(
    val id: Int,
    val name: String,
    val logoUrl: String,
    val storeOffers: List<StoreOfferItem>
)