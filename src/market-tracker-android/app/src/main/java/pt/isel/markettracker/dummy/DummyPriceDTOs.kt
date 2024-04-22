package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.price.StorePrice
import pt.isel.markettracker.http.models.price.CompanyPrices
import java.time.LocalDateTime

val dummyStorePrices = dummyStores.map {
    StorePrice(
        it,
        (80..130).random(),
        null,
        true,
        LocalDateTime.now().minusDays(1)
    )
}

val dummyCompanyPrices = dummyCompanies.map { company ->
    CompanyPrices(company.id, company.name, company.logoUrl, dummyStorePrices.filter { storePrice ->
        dummyStores.any { it.company.id == company.id && it.id == storePrice.id }
    })
}