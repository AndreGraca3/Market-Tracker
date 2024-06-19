package pt.isel.markettracker.repository.auth

import kotlinx.coroutines.flow.StateFlow
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.PriceAlertOutputModel
import pt.isel.markettracker.http.models.list.ShoppingListOutputModel

/**
 * Used for global authentication state management
 */
interface IAuthRepository {
    val authState: StateFlow<AuthEvent>

    fun isUserLoggedIn(): Boolean

    suspend fun getToken(): String?

    suspend fun setToken(token: String?)

    suspend fun setLists(lists: CollectionOutputModel<ShoppingListOutputModel>)

    suspend fun getLists(): CollectionOutputModel<ShoppingListOutputModel>

    suspend fun setAlerts(alerts: CollectionOutputModel<PriceAlertOutputModel>)
}