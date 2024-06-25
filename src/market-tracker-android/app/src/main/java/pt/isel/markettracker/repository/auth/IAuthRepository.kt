package pt.isel.markettracker.repository.auth

import kotlinx.coroutines.flow.StateFlow
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.market.price.PriceAlert

/**
 * Used for global authentication state management
 */
interface IAuthRepository {
    val authState: StateFlow<AuthState>

    suspend fun getOrGenerateDeviceId(): String

    suspend fun setToken(token: String?)

    suspend fun getToken(): String?

    fun setDetails(lists: List<ShoppingList>, alerts: List<PriceAlert>)
}