package pt.isel.markettracker.domain.model.market

data class Store(
    val id: Int,
    val name: String,
    val address: String,
    val city: City?,
    val company: Company,
    val isOnline: Boolean
) {
    fun toStoreInfo() = StoreInfo(id, name, address, city, isOnline)
}

data class StoreInfo(
    val id: Int,
    val name: String,
    val address: String,
    val city: City?,
    val isOnline: Boolean
)