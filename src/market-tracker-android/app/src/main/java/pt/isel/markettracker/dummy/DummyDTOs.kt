package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.City
import pt.isel.markettracker.domain.product.CompanyInfo
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.domain.product.StoreInfo

val dummyProducts = listOf(
    ProductInfo(
        "12345435",
        "Saloio Queijo mesmo do bom autentico do norte e mal cheiroso grande 230gr",
        "https://media.kabaz.pt/images/products/1/2/6/9/4/126946-1706041053.png",
        "Saloio",
        "Laticínios"
    ),
    ProductInfo(
        "84433442",
        "Artiach Filipinos Bolacha com Chocolate 200g",
        "https://media.kabaz.pt/images/products/0/6/5/6/5/6565-1683877053.png?w=240",
        "Artiach",
        "Mercearia"
    ),
    ProductInfo(
        "123654635",
        "Oreo Bolacha com Recheio de Baunilha 154g",
        "https://media.kabaz.pt/images/products/5/2/8/9/4/52894-1462285783.png?w=240",
        "Oreo",
        "Mercearia"
    ),
    ProductInfo(
        "65768768",
        "Nutella Creme de Avelãs e Cacau 400g",
        "https://media.kabaz.pt/images/products/1/1/7/7/6/117763-1681385790.png?w=600",
        "Nutella",
        "Mercearia"
    ),
    ProductInfo(
        "123653163325",
        "Coca-Cola Bebida Gaseificada 1.5L",
        "ad",
        "Coca-Cola",
        "Bebidas",
    ),
)

// Dummy data for city
val dummyCities = listOf(
    City(1, "Lisboa"),
    City(2, "Porto"),
    City(3, "Cacém")
)

// Dummy data for store prices

val dummyStores = listOf(
    StoreInfo(
        1,
        "Minipreço mas só de Lisboa",
        "Rua do Minipreço",
        dummyCities[0],
        false,
        CompanyInfo(
            1,
            "Minipreço",
            "https://www.kabaz.pt/stores/logo-minipreco.webp"
        )
    ),
    StoreInfo(
        2,
        "Continente",
        "www.continente.pt",
        null,
        true,
        CompanyInfo(
            2,
            "Continente",
            "https://www.kabaz.pt/stores/logo-continente.webp"
        )
    ),
    StoreInfo(
        3,
        "Pingo Doce",
        "Rua Portunhal Central",
        dummyCities[1],
        false,
        CompanyInfo(
            3,
            "Pingo Doce",
            "https://www.kabaz.pt/stores/logo-pingo-doce.webp"
        )
    ),
    StoreInfo(
        4,
        "Auchan",
        "Auchan Cacém de baixo",
        dummyCities[2],
        false,
        CompanyInfo(
            4,
            "Auchan",
            "https://www.kabaz.pt/stores/logo-auchan.webp"
        )
    )
)