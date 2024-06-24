package pt.isel.markettracker.http.service.operations.user

import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel

interface IUserService {
    suspend fun getUsers(username: String?): PaginatedResult<ClientItem>

    suspend fun getUser(id: String): Client

    suspend fun getAuthenticatedUser(): Client

    suspend fun createUser(input: UserCreationInputModel): StringIdOutputModel

    suspend fun updateUser( input: UserUpdateInputModel): Client

    suspend fun deleteUser()

    suspend fun registerDevice(token: String, deviceId: String)
}