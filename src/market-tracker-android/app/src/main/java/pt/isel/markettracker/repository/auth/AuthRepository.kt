package pt.isel.markettracker.repository.auth

import android.util.Log
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.first
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import javax.inject.Inject

class AuthRepository @Inject constructor(private val dataStore: DataStore<Preferences>) :
    IAuthRepository {

    private val TAG = "AuthRepository"

    private val tokenKey = stringPreferencesKey("token")

    private val _authState = MutableStateFlow<AuthState>(AuthState.Idle)
    override val authState
        get() = _authState.asStateFlow()

    override suspend fun getToken(): String? {
        val preferences = dataStore.data.first()
        return preferences[tokenKey]
    }

    override suspend fun setToken(token: String?) {
        dataStore.edit { preferences ->
            if (!token.isNullOrEmpty()) {
                preferences[tokenKey] = token
                Log.v(TAG, "Token is set")
            } else {
                preferences.remove(tokenKey)
                _authState.value = AuthState.Logout
                Log.v(TAG, "AuthState is logout")
            }
        }
    }

    override fun setDetails(lists: List<ShoppingList>, alerts: List<PriceAlert>) {
        // if (_authState.value !is AuthState.Idle) return
        _authState.value = AuthState.Loaded(lists, alerts)
        Log.v(TAG, "AuthState is loaded")
    }
}

sealed class AuthState {
    data object Idle : AuthState()
    data class Loaded(val lists: List<ShoppingList>, val alerts: List<PriceAlert>) : AuthState()
    data object Logout : AuthState()
}

fun AuthState.isLoggedIn() = this is AuthState.Loaded

fun AuthState.extractLists(): List<ShoppingList> =
    when (this) {
        is AuthState.Loaded -> lists
        else -> emptyList()
    }

fun AuthState.extractAlerts(): List<PriceAlert> =
    when (this) {
        is AuthState.Loaded -> alerts
        else -> emptyList()
    }