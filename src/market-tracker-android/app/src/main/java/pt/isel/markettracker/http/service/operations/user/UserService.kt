package pt.isel.markettracker.http.service.operations.user

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel
import pt.isel.markettracker.http.service.MarketTrackerService

private const val usersAllPath = "/clients"
private const val authenticatedUserPath = "$usersAllPath/me"
private fun buildUserByIdPath(id: String) = "$usersAllPath/$id"

class UserService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IUserService, MarketTrackerService() {

    override suspend fun getUsers(username: String?): PaginatedResult<ClientItem> {
        return requestHandler(
            path = usersAllPath,
            method = HttpMethod.GET,
            body = username
        )
    }

    override suspend fun getUser(id: String): Client {
        return requestHandler(
            path = buildUserByIdPath(id),
            method = HttpMethod.GET
        )
    }

    override suspend fun getAuthenticatedUser(): Client {
        return requestHandler(
            path = authenticatedUserPath,
            method = HttpMethod.GET
        )
    }

    override suspend fun createUser(
        name: String,
        username: String,
        email: String,
        password: String,
        avatar: String?
    ): String {
        return requestHandler<StringIdOutputModel>(
            path = usersAllPath,
            method = HttpMethod.POST,
            body = UserCreationInputModel(
                name = name,
                username = username,
                email = email,
                password = password,
                avatar = avatar
            )
        ).id
    }

    override suspend fun updateUser(name: String?, username: String?, avatar: String?): Client {
        return requestHandler(
            path = usersAllPath,
            method = HttpMethod.PUT,
            body = UserUpdateInputModel(
                name = name,
                username = username,
                avatar = avatar
            )
        )
    }

    override suspend fun deleteUser() {
        return requestHandler(
            path = usersAllPath,
            method = HttpMethod.DELETE
        )
    }
}