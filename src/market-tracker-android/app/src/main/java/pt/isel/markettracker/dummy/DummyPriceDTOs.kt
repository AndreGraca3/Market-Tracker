package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.Price
import pt.isel.markettracker.domain.model.market.price.Promotion
import pt.isel.markettracker.domain.model.market.price.StoreOffer
import java.time.LocalDateTime

val dummyStoreOffers = dummyStores.map {
    StoreOffer(
        it,
        Price(10, 10, Promotion(10, LocalDateTime.now()), LocalDateTime.now().minusDays(1)),
        true,
        LocalDateTime.now().minusDays(1)
    )
}

val dummyCompanyPrices = dummyCompanies.map { company ->
    CompanyPrices(company.id, company.name, company.logoUrl, dummyStoreOffers.filter { storePrice ->
        dummyStores.any { it.company.id == company.id && it.id == storePrice.store.id }
    }.map { it.toStoreOfferItem() })
}