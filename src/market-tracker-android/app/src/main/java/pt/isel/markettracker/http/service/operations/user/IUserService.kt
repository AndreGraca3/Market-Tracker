package pt.isel.markettracker.http.service.operations.user

import pt.isel.markettracker.http.models.IdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserOutputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel
import pt.isel.markettracker.http.models.user.UsersOutputModel
import java.util.UUID

interface IUserService {
    suspend fun getUsers(/* pagination + username*/): UsersOutputModel

    suspend fun getUser(): UserOutputModel

    suspend fun createUser(input: UserCreationInputModel): IdOutputModel

    suspend fun updateUser(input: UserUpdateInputModel): UserOutputModel

    suspend fun deleteUser(): IdOutputModel
}