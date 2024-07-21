package pt.isel.markettracker.repository.auth

import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.market.inventory.product.ProductItem
import pt.isel.markettracker.domain.model.market.price.PriceAlert

data class Details(
    val lists: List<ShoppingList>,
    val alerts: List<PriceAlert>,
    val favorites: List<ProductItem>,
)