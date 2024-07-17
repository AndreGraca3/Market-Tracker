package pt.isel.markettracker.http.models.list

data class ListEntryCreationInputModel(
    val productId: String,
    val storeId: Int,
    val quantity: Int,
)
