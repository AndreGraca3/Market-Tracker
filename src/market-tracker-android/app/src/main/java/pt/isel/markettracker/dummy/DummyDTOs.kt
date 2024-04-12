package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.product.CompanyInfo
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.domain.product.StorePriceData

val dummyProducts = listOf(
    ProductInfo(
        "12345435",
        "Saloio Queijo do bom Product 1 230gr",
        "https://media.kabaz.pt/images/products/1/2/6/9/4/126946-1706041053.png",
        "Saloio",
        "Laticínios",
        StorePriceData(
            1,
            "Continente",
            CompanyInfo(
                1,
                "Continente",
                "https://www.kabaz.pt/stores/logo-continente.webp"
            ),
            489
        )
    ),
    ProductInfo(
        "84433442",
        "Artiach Filipinos Bolacha com Chocolate 200g",
        "https://media.kabaz.pt/images/products/0/6/5/6/5/6565-1683877053.png?w=240",
        "Artiach",
        "Mercearia",
        StorePriceData(
            1,
            "Continente Online",
            CompanyInfo(
                1,
                "Continente",
                "https://www.kabaz.pt/stores/logo-continente.webp"
            ),
            109
        )
    ),
    ProductInfo(
        "123654635",
        "Oreo Bolacha com Recheio de Baunilha 154g",
        "https://media.kabaz.pt/images/products/5/2/8/9/4/52894-1462285783.png?w=240",
        "Oreo",
        "Mercearia",
        StorePriceData(
            1,
            "Pingo Doce Amadora",
            CompanyInfo(
                1,
                "Pingo Doce",
                "https://www.kabaz.pt/stores/logo-pingodoce.webp"
            ),
            299
        )
    ),
    ProductInfo(
        "65768768",
        "Nutella Creme de Avelãs e Cacau 400g",
        "https://media.kabaz.pt/images/products/1/1/7/7/6/117763-1681385790.png?w=600",
        "Nutella",
        "Mercearia",
        StorePriceData(
            1,
            "Auchan Massamá",
            CompanyInfo(
                1,
                "Auchan",
                "https://www.kabaz.pt/stores/logo-auchan.webp"
            ),
            545
        )
    ),
    ProductInfo(
        "123653163325",
        "Coca-Cola Bebida Gaseificada 1.5L",
        "ad",
        "Coca-Cola",
        "Bebidas",
        StorePriceData(
            1,
            "Pingo Doce Amadora",
            CompanyInfo(
                1,
                "MiniPreço",
                "https://www.kabaz.pt/stores/logo-minipreco.webp"
            ),
            299
        )
    ),
)