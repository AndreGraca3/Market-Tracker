package pt.isel.markettracker.http.service.operations.user

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.user.User
import pt.isel.markettracker.dummy.dummyUsers
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
        private val USERS_REQUEST_URL = URL("${MT_API_URL}/users/all")
        private val USER_REQUEST_URL = URL("${MT_API_URL}/users")
    }

    override suspend fun getUsers(username: String?): UsersOutputModel {
        /*return requestHandler<UsersOutputModel>(
            request = Request.Builder().buildRequest(
                url = USERS_REQUEST_URL,
                method = HttpMethod.GET
            )
        )*/
        delay(1000)
        val newUsers = if (username != null) {
            dummyUsers.filter { it.username.contains(username) }
        } else dummyUsers

        return UsersOutputModel(
            users = newUsers.map { user -> user.toUserOutputModel() },
            total = newUsers.size
        )
    }

    override suspend fun getUser(id: String): UserOutputModel {
        //return requestHandler<UserOutputModel>(
        //    request = Request.Builder().buildRequest(
        //        url = USER_REQUEST_URL,
        //        method = HttpMethod.GET
        //    )
        //)
        delay(1000)
        return dummyUsers.first { it.id == id }.toUserOutputModel()
    }

    override suspend fun createUser(input: UserCreationInputModel): IdOutputModel {
        /*return requestHandler<IdOutputModel>(
            request = Request.Builder().buildRequest(
                url = USER_REQUEST_URL,
                input = input,
                method = HttpMethod.POST
            )
        )*/
        val id = dummyUsers.last().id + 1
        dummyUsers.add(
            User(
                id = id,
                username = input.username,
                name = input.name,
                email = input.email,
                password = input.password
            )
        )
        return IdOutputModel(id)
    }

    override suspend fun updateUser(id: String, input: UserUpdateInputModel): UserOutputModel {
        //return requestHandler<UserOutputModel>(
        //    request = Request.Builder().buildRequest(
        //        url = USER_REQUEST_URL,
        //        input = input,
        //        method = HttpMethod.PUT
        //    )
        //)
        dummyUsers.map {
            if (it.id == id) {
                UserOutputModel(
                    id = id,
                    name = input.name ?: it.name,
                    username = input.username ?: it.username,
                    email = it.email
                )
            }
        }
        return dummyUsers.first { it.id == id }.toUserOutputModel()
    }

    override suspend fun updateUserAvatar(id: String, avatar: String) {
        dummyUsers.map {
            if (it.id == id) {
                it.avatar = avatar
            }
        }
    }

    override suspend fun deleteUser(id: String): IdOutputModel {
        //return requestHandler<IdOutputModel>(
        //    request = Request.Builder().buildRequest(
        //        url = USER_REQUEST_URL,
        //        method = HttpMethod.DELETE
        //    )
        //)
        dummyUsers.removeIf { it.id == id } // verify if true
        return IdOutputModel(id)
    }

    private fun User.toUserOutputModel(): UserOutputModel {
        return UserOutputModel(
            id = this.id,
            name = this.name,
            username = this.username,
            email = this.email
        )
    }
}