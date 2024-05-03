package pt.isel.markettracker.domain.infrastruture

import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import kotlinx.coroutines.flow.first

class TokenRepository(
    private val datastore: DataStore<Preferences>
) : ITokenRepository {

    companion object {
        private const val TOKEN_KEY = "token"
        private val tokenKey = stringPreferencesKey(TOKEN_KEY)
    }

    override suspend fun getToken(): String? {
        val preferences = datastore.data.first()
        return preferences[tokenKey]
    }

    override suspend fun setToken(tokenValue: String) {
        datastore.edit { preferences ->
            preferences[tokenKey] = tokenValue
        }
    }

    override suspend fun clearToken(tokenValue: String) {
        datastore.edit { preferences ->
            preferences.remove(tokenKey)
        }
    }

    override suspend fun isLoggedIn(): Boolean {
        return getToken() != null
    }
}