package pt.isel.markettracker.http.service.operations.token

import com.google.gson.Gson
import okhttp3.OkHttpClient
import okhttp3.Request
import pt.isel.markettracker.http.models.token.TokenCreationInputModel
import pt.isel.markettracker.http.service.MarketTrackerService
import java.net.URL
import java.util.UUID

class TokenService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : ITokenService, MarketTrackerService() {

    companion object {
        private val TOKEN_REQUEST_URL = URL("/tokens")
    }

    override suspend fun createToken(input: TokenCreationInputModel) {
        // requestHandler<UUID>()
        requestHandler<Unit>(
            request = Request.Builder().buildRequest(
                url = TOKEN_REQUEST_URL,
                input = input,
                method = HttpMethod.PUT
            )
        )
    }

    override suspend fun deleteToken(tokenValue: UUID) {
        requestHandler<Unit>(
            request = Request.Builder().buildRequest(
                url = TOKEN_REQUEST_URL,
                input = tokenValue,
                method = HttpMethod.DELETE
            )
        )
    }
}