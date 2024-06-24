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

private const val usersBasePath = "/clients"
private const val authenticatedUserPath = "$usersBasePath/me"
private fun buildUserByIdPath(id: String) = "$usersBasePath/$id"

private const val registerDevicePath = "$authenticatedUserPath/register-push-notifications"

class UserService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IUserService, MarketTrackerService() {

    override suspend fun getUsers(username: String?): PaginatedResult<ClientItem> {
        return requestHandler(
            path = usersBasePath,
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

    override suspend fun createUser(input: UserCreationInputModel): StringIdOutputModel {
        return requestHandler(
            path = usersBasePath,
            method = HttpMethod.POST,
            body = input
        )
    }

    override suspend fun updateUser(input: UserUpdateInputModel): Client {
        return requestHandler(
            path = usersBasePath,
            method = HttpMethod.PUT,
            body = input
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