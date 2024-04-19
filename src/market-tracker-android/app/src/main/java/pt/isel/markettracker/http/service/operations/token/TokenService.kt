package pt.isel.markettracker.http.service.operations.token

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.token.Token
import pt.isel.markettracker.dummy.dummyTokens
import pt.isel.markettracker.dummy.dummyUsers
import pt.isel.markettracker.http.models.token.TokenCreationInputModel
import pt.isel.markettracker.http.models.token.TokenOutputModel
import pt.isel.markettracker.http.service.MarketTrackerService
import java.net.URL
import java.util.UUID

class TokenService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : ITokenService, MarketTrackerService() {

    companion object {
        private val TOKEN_REQUEST_URL = URL("${MT_API_URL}/tokens")
    }

    override suspend fun createToken(input: TokenCreationInputModel): TokenOutputModel {
        //requestHandler<UUID>(
        //    request = Request.Builder().buildRequest(
        //        url = TOKEN_REQUEST_URL,
        //        input = input,
        //        method = HttpMethod.PUT
        //    )
        //)
        val user =
            dummyUsers.find { it.email == input.email && it.password == input.password }
        return if (user != null) {
            val newToken = "token"
            dummyTokens.add(
                Token(
                    tokenValue = newToken,
                    userId = user.id
                )
            )
            TokenOutputModel(newToken)
        } else {
            TokenOutputModel("")
        }
    }

    override suspend fun deleteToken(tokenValue: String) {
        //requestHandler<Unit>(
        //    request = Request.Builder().buildRequest(
        //        url = TOKEN_REQUEST_URL,
        //        input = tokenValue,
        //        method = HttpMethod.DELETE
        //    )
        //)
        dummyTokens.removeIf { it.tokenValue == tokenValue}
    }
}