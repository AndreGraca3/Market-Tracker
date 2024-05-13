package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.market.City
import pt.isel.markettracker.domain.model.market.Company
import pt.isel.markettracker.domain.model.market.Store

// Dummy data for city
val dummyCities = listOf(
    City(1, "Lisboa"),
    City(2, "Porto"),
    City(3, "Cacém")
)

val dummyCompanies = listOf(
    Company(
        1,
        "Minipreço",
        "https://www.kabaz.pt/stores/logo-minipreco.webp"
    ),
    Company(
        2,
        "Continente",
        "https://www.kabaz.pt/stores/logo-continente.webp"
    ),
    Company(
        3,
        "Pingo Doce",
        "https://www.kabaz.pt/stores/logo-pingodoce.webp"
    ),
    Company(
        4,
        "Auchan",
        "https://www.kabaz.pt/stores/logo-auchan.webp"
    )
)

// Dummy data for store prices

val dummyStores = listOf(
    Store(
        1,
        "Minipreço mas só de Lisboa",
        "Rua do Minipreço",
        dummyCities[0],
        dummyCompanies[0],
        false
    ),
    Store(
        2,
        "Continente Online",
        "www.continente.pt",
        null,
        dummyCompanies[1],
        true
    ),
    Store(
        3,
        "Pingo Doce Online",
        "www.mercadao.pt",
        null,
        dummyCompanies[2],
        true
    ),
    Store(
        4,
        "Pingo Doce",
        "Rua Portunhal Central",
        dummyCities[1],
        dummyCompanies[2],
        false
    ),
    Store(
        5,
        "Auchan da calçada do rico e belo Cacém",
        "Auchan Cacém de baixo",
        dummyCities[2],
        dummyCompanies[3],
        false
    )
)