package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.market.inventory.Brand
import pt.isel.markettracker.domain.model.market.inventory.Category
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductUnit

val dummyCategories = listOf(
    Category(1, "Mercearia"),
    Category(2, "Laticínios")
)

val dummyBrands = listOf(
    Brand(1, "Saloio"),
    Brand(2, "Artiach"),
    Brand(3, "Oreo"),
    Brand(4, "Nutella"),
    Brand(5, "Coca-Cola"),
    Brand(6, "Cuétara")
)

val dummyProducts = listOf(
    Product(
        "12345435",
        "Saloio Queijo mesmo do bom autentico do norte e mal cheiroso grande 230gr",
        "https://media.kabaz.pt/images/products/1/2/6/9/4/126946-1706041053.png",
        1,
        ProductUnit.GRAMS,
        3.5,
        dummyBrands[0],
        dummyCategories[1]
    ),
    Product(
        "84433442",
        "Artiach Filipinos Bolacha com Chocolate 200g",
        "https://media.kabaz.pt/images/products/0/6/5/6/5/6565-1683877053.png",
        1,
        ProductUnit.GRAMS,
        2.6,
        dummyBrands[1],
        dummyCategories[0]
    ),
    Product(
        "123654635",
        "Oreo Bolacha com Recheio de Baunilha 154g",
        "https://media.kabaz.pt/images/products/5/2/8/9/4/52894-1462285783.png",
        1,
        ProductUnit.GRAMS,
        4.5,
        dummyBrands[2],
        dummyCategories[0]
    ),
    Product(
        "65768768",
        "Nutella Creme de Avelãs e Cacau 400g",
        "https://media.kabaz.pt/images/products/1/1/7/7/6/117763-1681385790.png",
        1,
        ProductUnit.GRAMS,
        2.5,
        dummyBrands[3],
        dummyCategories[0]
    ),
    Product(
        "123653163325",
        "Coca-Cola Bebida Gaseificada 1.5L",
        "wrong_image_url",
        1,
        ProductUnit.LITERS,
        4.5,
        dummyBrands[4],
        dummyCategories[0]
    ),
    Product(
        "8434165446984",
        "Cuétara Bolacha Maria 200g",
        "https://media.kabaz.pt/images/products/2/8/4/4/2/28442-1456160198.png",
        1,
        ProductUnit.GRAMS,
        4.5,
        dummyBrands[5],
        dummyCategories[0]
    )
)