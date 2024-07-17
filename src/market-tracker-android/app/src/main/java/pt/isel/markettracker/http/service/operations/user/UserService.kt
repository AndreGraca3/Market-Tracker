package pt.isel.markettracker.http.service.operations.user

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.domain.model.account.PaginatedClientItem
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel
import pt.isel.markettracker.http.service.MarketTrackerService

private fun buildUsersPath(
    page: Int,
    itemsPerPage: Int?,
    username: String?,
) = "${usersBasePath}?page=$page" +
        itemsPerPage?.let { "&itemsPerPage=$it" }.orEmpty() +
        username?.let { "&username=$it" }.orEmpty()

private const val usersBasePath = "/clients"
private const val authenticatedUserPath = "$usersBasePath/me"
private fun buildUserByIdPath(id: String) = "$usersBasePath/$id"

private const val registerDevicePath = "$authenticatedUserPath/register-push-notifications"

class UserService(
    override val httpClient: OkHttpClient,
    override val gson: Gson,
) : IUserService, MarketTrackerService() {

    override suspend fun getUsers(
        page: Int,
        itemsPerPage: Int?,
        username: String?,
    ): PaginatedClientItem {
        return requestHandler(
            path = buildUsersPath(page, itemsPerPage, username),
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
        avatar: String?,
    ): String {
        return requestHandler<StringIdOutputModel>(
            path = usersBasePath,
            method = HttpMethod.POST,
            body = UserCreationInputModel(
                name = name,
                username = username,
                email = email,
                password = password,
                avatar = avatar
            )
        ).value
    }

    override suspend fun updateUser(name: String?, username: String?, avatar: String?): Client {
        return requestHandler(
            path = usersBasePath,
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
            path = usersBasePath,
            method = HttpMethod.DELETE
        )
    }

    override suspend fun registerDevice(token: String, deviceId: String) {
        return requestHandler(
            path = registerDevicePath,
            method = HttpMethod.POST,
            body = mapOf("firebaseToken" to token, "deviceId" to deviceId)
        )
    }
}