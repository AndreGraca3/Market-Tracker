package pt.isel.markettracker.repository.auth

import android.util.Log
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.first
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.PriceAlertOutputModel
import pt.isel.markettracker.http.models.list.ShoppingListOutputModel
import pt.isel.markettracker.repository.auth.GsonSerializer.AlertsGsonSerializer
import pt.isel.markettracker.repository.auth.GsonSerializer.ListsGsonSerializer
import javax.inject.Inject

class AuthRepository @Inject constructor(
    private val dataStore: DataStore<Preferences>
) : IAuthRepository {

    private val tokenKey = stringPreferencesKey("token")
    private val alertsKey = stringPreferencesKey("alerts")
    private val listsKey = stringPreferencesKey("listsKey")

    private val _authState = MutableStateFlow<AuthEvent>(AuthEvent.Idle)
    override val authState
        get() = _authState.asStateFlow()

    override fun isUserLoggedIn(): Boolean {
        return _authState.value is AuthEvent.Login
    }

    override suspend fun getToken(): String? {
        val preferences = dataStore.data.first()
        return preferences[tokenKey]
    }

    override suspend fun setToken(token: String?) {
        dataStore.edit { preferences ->
            if (!token.isNullOrEmpty()) {
                preferences[tokenKey] = token
                _authState.value = AuthEvent.Login
                Log.v("User", "Token state is login")
            } else {
                preferences.remove(tokenKey)
                _authState.value = AuthEvent.Logout
                Log.v("User", "Token state is logout")
            }
        }
    }

    override suspend fun setLists(lists: CollectionOutputModel<ShoppingListOutputModel>) {
        if (!isUserLoggedIn()) return
        dataStore.edit { preferences ->
            if (lists.items.isNotEmpty()) {
                preferences[listsKey] = ListsGsonSerializer.serialize(lists)
                Log.v("User", "Updated lists")
            } else {
                preferences.remove(listsKey)
                Log.v("User", "Removed lists")
            }
        }
    }

    override suspend fun getLists(): CollectionOutputModel<ShoppingListOutputModel> {
        val preferences = dataStore.data.first()
        val listsPreferences = preferences[listsKey] ?: return CollectionOutputModel(emptyList())
        return ListsGsonSerializer.deserialize(listsPreferences)
    }

    override suspend fun setAlerts(alerts: CollectionOutputModel<PriceAlertOutputModel>) {
        if (!isUserLoggedIn()) return
        dataStore.edit { preferences ->
            if (alerts.items.isNotEmpty()) {
                preferences[alertsKey] = AlertsGsonSerializer.serialize(alerts)
                Log.v("User", "Updated alerts")
            } else {
                preferences.remove(alertsKey)
                Log.v("User", "Removed alerts")
            }
        }
    }
}

sealed class AuthEvent {
    data object Idle : AuthEvent()
    data object Login : AuthEvent()
    data object Logout : AuthEvent()
}