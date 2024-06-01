package pt.isel.markettracker.http.service.operations.user

import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel

interface IUserService {
    suspend fun getUsers(username: String?): PaginatedResult<Client>

    suspend fun getUser(id: String): Client

    suspend fun createUser(input: UserCreationInputModel): String // "create client"

    suspend fun updateUser(id: String, input: UserUpdateInputModel): Client

    suspend fun updateUserAvatar(id: String, avatar: String)

    suspend fun deleteUser(id: String): StringIdOutputModel
}