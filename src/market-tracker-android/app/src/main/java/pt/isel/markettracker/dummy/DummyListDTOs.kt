package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.domain.model.list.listEntry.ListEntryOffer
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import java.time.LocalDateTime
import java.util.UUID

val dummyShoppingListSocial = mutableListOf(
    ShoppingListSocial(
        1,
        "Festa de aniversário",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        0,
        isOwner = true
    ),
    //ShoppingListSocial(
    //    2,
    //    "Ano Novo",
    //    null,
    //    LocalDateTime.now().minusDays(2),
    //    UUID(1, 1),
    //    1,
    //    isOwner = true
    //),
    //ShoppingListSocial(
    //    3,
    //    "Compras Semanais",
    //    null,
    //    LocalDateTime.now().minusDays(2),
    //    UUID(1, 1),
    //    1,
    //    isOwner = false
    //),
    //ShoppingListSocial(
    //    4,
    //    "Compras Mensais",
    //    LocalDateTime.now().minusDays(1),
    //    LocalDateTime.now().minusDays(2),
    //    UUID(1, 1),
    //    5,
    //    isOwner = false
    //),
    //ShoppingListSocial(
    //    5,
    //    "Presentes de Natal",
    //    LocalDateTime.now().minusDays(1),
    //    LocalDateTime.now().minusDays(2),
    //    UUID(1, 1),
    //    2,
    //    isOwner = false
    //),
    //ShoppingListSocial(
    //    6,
    //    "Aperitivos para a festa",
    //    LocalDateTime.now().minusDays(1),
    //    LocalDateTime.now().minusDays(2),
    //    UUID(1, 1),
    //    7,
    //    isOwner = false
    //),
    //ShoppingListSocial(
    //    7,
    //    "Coisas que provavelmente não vou comprar",
    //    LocalDateTime.now().minusDays(1),
    //    LocalDateTime.now().minusDays(2),
    //    UUID(1, 1),
    //    4,
    //    isOwner = false
    //)
)

private val entryOfferList = mutableListOf(
    ListEntryOffer(
        ProductOffer(
            dummyProducts[0],
            dummyStorePrices[0]
        ),
        1
    ),
    ListEntryOffer(
        ProductOffer(
            dummyProducts[1],
            dummyStorePrices[1]
        ),
        2
    ),
)

val dummyShoppingListEntries = ShoppingListEntries(
    entryOfferList,
    entryOfferList.sumOf { it.productOffer.storePrice.priceData.finalPrice * it.quantity },
    entryOfferList.sumOf { it.quantity }
)