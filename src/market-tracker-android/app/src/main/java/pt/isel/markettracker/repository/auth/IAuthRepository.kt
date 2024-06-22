package pt.isel.markettracker.repository.auth

import kotlinx.coroutines.flow.StateFlow
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.market.price.PriceAlert

/**
 * Used for global authentication state management
 */
interface IAuthRepository {
    val authState: StateFlow<AuthEvent>

    fun isUserLoggedIn(): Boolean

    suspend fun getToken(): String?

    suspend fun setToken(token: String?)

    fun setLists(lists: List<ShoppingList>)

    fun getLists(): List<ShoppingList>

    fun setAlerts(alerts: List<PriceAlert>)

    fun getAlerts(): List<PriceAlert>
}