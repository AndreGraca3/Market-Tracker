package pt.isel.markettracker.http.models.price

import pt.isel.markettracker.domain.price.StorePrice

data class CompanyPrices(
    val id: Int,
    val name: String,
    val logoUrl: String,
    val stores: List<StorePrice>
)