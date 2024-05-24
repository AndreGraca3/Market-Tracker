package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.market.StoreInfo
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.Price
import pt.isel.markettracker.domain.model.market.price.Promotion
import pt.isel.markettracker.domain.model.market.price.StoreOfferItem
import java.time.LocalDateTime

val dummyStoreOffers = dummyStores.map {
    StoreOfferItem(
        StoreInfo(it.id, it.name, it.address, it.city, it.isOnline),
        Price(10, 10, Promotion(10, LocalDateTime.now()), LocalDateTime.now().minusDays(1)),
        true,
        LocalDateTime.now().minusDays(1)
    )
}

val dummyCompanyPrices = dummyCompanies.map { company ->
    CompanyPrices(company, dummyStoreOffers.filter { storePrice ->
        dummyStores.any { it.company.id == company.id && it.id == storePrice.store.id }
    })
}