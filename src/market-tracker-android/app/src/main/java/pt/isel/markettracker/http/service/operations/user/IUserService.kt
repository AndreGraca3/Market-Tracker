package pt.isel.markettracker.http.service.operations.user

import pt.isel.markettracker.http.models.IdOutputModel
import pt.isel.markettracker.http.models.UserCreationInputModel

interface IUserService {
    suspend fun createUser(input: UserCreationInputModel): IdOutputModel
}