package pt.isel.markettracker.domain.infrastruture

interface ITokenRepository {

    suspend fun getToken(): String? // UUID

    suspend fun setToken(tokenValue: String)

    suspend fun clearToken(tokenValue: String)

    suspend fun isLoggedIn(): Boolean
}