package pt.isel.markettracker.domain.model.market

data class Store(
    val id: Int,
    val name: String,
    val address: String,
    val city: City?,
    val company: Company,
    val isOnline: Boolean = city == null
)