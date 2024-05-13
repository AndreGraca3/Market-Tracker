package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.market.price.StorePrice
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.Price
import java.time.LocalDateTime

val dummyStorePrices = dummyStores.map {
    StorePrice(
        it,
        Price(10, 10, null, LocalDateTime.now().minusDays(1)),
        true,
        LocalDateTime.now().minusDays(1)
    )
}

val dummyCompanyPrices = dummyCompanies.map { company ->
    CompanyPrices(company, dummyStorePrices.filter { storePrice ->
        dummyStores.any { it.company.id == company.id && it.id == storePrice.store.id }
    })
}