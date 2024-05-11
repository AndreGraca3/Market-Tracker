package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.city.City
import pt.isel.markettracker.domain.model.product.CompanyInfo
import pt.isel.markettracker.domain.model.product.StoreInfo

// Dummy data for city
val dummyCities = listOf(
    City(1, "Lisboa"),
    City(2, "Porto"),
    City(3, "Cacém")
)

val dummyCompanies = listOf(
    CompanyInfo(
        1,
        "Minipreço",
        "https://www.kabaz.pt/stores/logo-minipreco.webp"
    ),
    CompanyInfo(
        2,
        "Continente",
        "https://www.kabaz.pt/stores/logo-continente.webp"
    ),
    CompanyInfo(
        3,
        "Pingo Doce",
        "https://www.kabaz.pt/stores/logo-pingodoce.webp"
    ),
    CompanyInfo(
        4,
        "Auchan",
        "https://www.kabaz.pt/stores/logo-auchan.webp"
    )
)

// Dummy data for store prices

val dummyStores = listOf(
    StoreInfo(
        1,
        "Minipreço mas só de Lisboa",
        "Rua do Minipreço",
        dummyCities[0],
        false,
        dummyCompanies[0]
    ),
    StoreInfo(
        2,
        "Continente Online",
        "www.continente.pt",
        null,
        true,
        dummyCompanies[1]
    ),
    StoreInfo(
        3,
        "Pingo Doce Online",
        "www.mercadao.pt",
        null,
        true,
        dummyCompanies[2]
    ),
    StoreInfo(
        4,
        "Pingo Doce",
        "Rua Portunhal Central",
        dummyCities[1],
        false,
        dummyCompanies[2]
    ),
    StoreInfo(
        5,
        "Auchan da calçada do rico e belo Cacém",
        "Auchan Cacém de baixo",
        dummyCities[2],
        false,
        dummyCompanies[3]
    )
)