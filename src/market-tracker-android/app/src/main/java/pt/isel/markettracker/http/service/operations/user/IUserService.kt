package pt.isel.markettracker.http.service.operations.user

import pt.isel.markettracker.http.models.IdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserOutputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel
import pt.isel.markettracker.http.models.user.UsersOutputModel

interface IUserService {
    suspend fun getUsers(username: String?): UsersOutputModel

    suspend fun getUser(id: String): UserOutputModel

    suspend fun createUser(input: UserCreationInputModel): IdOutputModel // "create client"

    suspend fun updateUser(id: String, input: UserUpdateInputModel): UserOutputModel

    suspend fun updateUserAvatar(id: String, avatar: String)

    suspend fun deleteUser(id: String): IdOutputModel
}