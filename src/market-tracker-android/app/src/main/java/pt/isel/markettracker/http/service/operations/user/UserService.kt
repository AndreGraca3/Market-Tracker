package pt.isel.markettracker.http.service.operations.user

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.http.models.IntIdOutputModel
import pt.isel.markettracker.http.models.UserCreationInputModel
import pt.isel.markettracker.http.service.MarketTrackerService

class UserService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IUserService, MarketTrackerService() {
    override suspend fun createUser(input: UserCreationInputModel): IntIdOutputModel {
        TODO("Not yet implemented")
    }
}