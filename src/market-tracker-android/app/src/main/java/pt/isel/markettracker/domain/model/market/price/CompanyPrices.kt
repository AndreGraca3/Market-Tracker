package pt.isel.markettracker.domain.model.market.price

import pt.isel.markettracker.domain.model.market.Company

data class CompanyPrices(
    val company: Company,
    val storeOffers: List<StoreOfferItem>
)