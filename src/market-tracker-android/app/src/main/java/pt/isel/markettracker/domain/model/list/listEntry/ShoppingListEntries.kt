package pt.isel.markettracker.domain.model.list.listEntry

data class ShoppingListEntries(
    val entries: List<ListEntryOffer>,
    val totalPrice: Int,
    val totalProducts: Int
)