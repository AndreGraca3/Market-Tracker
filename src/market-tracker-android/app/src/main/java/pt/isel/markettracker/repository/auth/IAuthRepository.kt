package pt.isel.markettracker.repository.auth

import kotlinx.coroutines.flow.StateFlow

/**
 * Used for global authentication state management
 */
interface IAuthRepository {
    val authState: StateFlow<AuthEvent>

    fun isUserLoggedIn(): Boolean

    suspend fun getToken(): String?

    suspend fun setToken(token: String?)
}