package pt.isel.markettracker.http.service.operations.user

import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.domain.model.account.PaginatedClientItem

interface IUserService {
    suspend fun getUsers(
        page: Int,
        itemsPerPage: Int? = null,
        username: String? = null,
    ): PaginatedClientItem

    suspend fun getUser(id: String): Client

    suspend fun getAuthenticatedUser(): Client

    suspend fun createUser(
        name: String,
        username: String,
        email: String,
        password: String,
        avatar: String? = null,
    ): String

    suspend fun updateUser(
        name: String? = null,
        username: String? = null,
        avatar: String? = null,
    ): Client

    suspend fun deleteUser()

    suspend fun registerDevice(token: String, deviceId: String)
}