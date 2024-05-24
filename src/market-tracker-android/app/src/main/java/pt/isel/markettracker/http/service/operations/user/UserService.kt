package pt.isel.markettracker.http.service.operations.user

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.dummy.dummyClients
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.models.user.UserUpdateInputModel
import pt.isel.markettracker.http.service.MarketTrackerService
import java.net.URL
import java.time.LocalDateTime

class UserService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IUserService, MarketTrackerService() {

    companion object {
        private val USERS_REQUEST_URL = URL("${MT_API_URL}/users/all")
        private val USER_REQUEST_URL = URL("${MT_API_URL}/users")
    }

    override suspend fun getUsers(username: String?): PaginatedResult<Client> {
        /*return requestHandler<UsersOutputModel>(
            request = Request.Builder().buildRequest(
                url = USERS_REQUEST_URL,
                method = HttpMethod.GET
            )
        )*/
        delay(1000)
        val newUsers = if (username != null) {
            dummyClients.filter { it.username.contains(username) }
        } else dummyClients

        return PaginatedResult(
            items = newUsers,
            currentPage = 1,
            itemsPerPage = newUsers.size,
            totalItems = newUsers.size,
            totalPages = 1,
        )
    }

    override suspend fun getUser(id: String): Client {
        //return requestHandler<UserOutputModel>(
        //    request = Request.Builder().buildRequest(
        //        url = USER_REQUEST_URL,
        //        method = HttpMethod.GET
        //    )
        //)
        delay(1000)
        return dummyClients.first { it.id == id }
    }

    override suspend fun createUser(input: UserCreationInputModel): String {
        /*return requestHandler<StringIdOutputModel>(
            request = Request.Builder().buildRequest(
                url = USER_REQUEST_URL,
                input = input,
                method = HttpMethod.POST
            )
        )*/
        val id = dummyClients.last().id + 1
        dummyClients.add(
            Client(
                id = id,
                username = input.username,
                name = input.name,
                email = input.email,
                password = input.password,
                "avatar",
                LocalDateTime.now()
            )
        )
        return id
    }

    override suspend fun updateUser(id: String, input: UserUpdateInputModel): Client {
        //return requestHandler<UserOutputModel>(
        //    request = Request.Builder().buildRequest(
        //        url = USER_REQUEST_URL,
        //        input = input,
        //        method = HttpMethod.PUT
        //    )
        //)
        return dummyClients.first { it.id == id }
    }

    override suspend fun updateUserAvatar(id: String, avatar: String) {
        dummyClients.map {
            if (it.id == id) {
                it.avatar = avatar
            }
        }
    }

    override suspend fun deleteUser(id: String): StringIdOutputModel {
        //return requestHandler<IdOutputModel>(
        //    request = Request.Builder().buildRequest(
        //        url = USER_REQUEST_URL,
        //        method = HttpMethod.DELETE
        //    )
        //)
        dummyClients.removeIf { it.id == id } // verify if true
        return StringIdOutputModel(id)
    }
}