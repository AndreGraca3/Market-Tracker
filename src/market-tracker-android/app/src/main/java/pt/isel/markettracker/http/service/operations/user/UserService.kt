package pt.isel.markettracker.http.service.operations.user

import com.google.gson.Gson
import okhttp3.OkHttpClient
import okhttp3.Request
import pt.isel.markettracker.http.models.IdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserOutputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel
import pt.isel.markettracker.http.models.user.UsersOutputModel
import pt.isel.markettracker.http.service.MarketTrackerService
import java.net.URL

class UserService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IUserService, MarketTrackerService() {

    companion object {
        private val USERS_REQUEST_URL = URL("/users/all")
        private val USER_REQUEST_URL = URL("/users")
    }

    override suspend fun getUsers(): UsersOutputModel {
        return requestHandler<UsersOutputModel>(
            request = Request.Builder().buildRequest(
                url = USERS_REQUEST_URL,
                method = HttpMethod.GET
            )
        )
    }

    override suspend fun getUser(): UserOutputModel {
        return requestHandler<UserOutputModel>(
            request = Request.Builder().buildRequest(
                url = USER_REQUEST_URL,
                method = HttpMethod.GET
            )
        )
    }

    override suspend fun createUser(input: UserCreationInputModel): IdOutputModel {
        return requestHandler<IdOutputModel>(
            request = Request.Builder().buildRequest(
                url = USER_REQUEST_URL,
                input = input,
                method = HttpMethod.POST
            )
        )
    }

    override suspend fun updateUser(input: UserUpdateInputModel): UserOutputModel {
        return requestHandler<UserOutputModel>(
            request = Request.Builder().buildRequest(
                url = USER_REQUEST_URL,
                input = input,
                method = HttpMethod.PUT
            )
        )
    }

    override suspend fun deleteUser(): IdOutputModel {
        return requestHandler<IdOutputModel>(
            request = Request.Builder().buildRequest(
                url = USER_REQUEST_URL,
                method = HttpMethod.DELETE
            )
        )
    }
}