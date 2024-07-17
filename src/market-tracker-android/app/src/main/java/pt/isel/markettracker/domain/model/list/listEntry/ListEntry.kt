package pt.isel.markettracker.domain.model.list.listEntry

import pt.isel.markettracker.domain.model.market.Store
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer

data class ListEntry(
    val id: String,
    val productId: String,
    val storeId: Store?,
    val quantity: Int,
)

data class ListEntryOffer(val id: String, val productOffer: ProductOffer, val quantity: Int)