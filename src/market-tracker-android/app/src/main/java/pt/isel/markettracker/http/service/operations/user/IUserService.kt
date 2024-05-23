package pt.isel.markettracker.http.service.operations.user

import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.account.User
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel

interface IUserService {
    suspend fun getUsers(username: String?): PaginatedResult<User>

    suspend fun getUser(id: String): User

    suspend fun createUser(input: UserCreationInputModel): String // "create client"

    suspend fun updateUser(id: String, input: UserUpdateInputModel): User

    suspend fun updateUserAvatar(id: String, avatar: String)

    suspend fun deleteUser(id: String): StringIdOutputModel
}