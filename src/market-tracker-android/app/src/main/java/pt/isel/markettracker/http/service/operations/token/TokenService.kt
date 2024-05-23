package pt.isel.markettracker.http.service.operations.token

import android.util.Log
import com.google.firebase.messaging.FirebaseMessaging
import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.account.Token
import pt.isel.markettracker.dummy.dummyTokens
import pt.isel.markettracker.dummy.dummyUsers
import pt.isel.markettracker.http.models.token.TokenCreationInputModel
import pt.isel.markettracker.http.models.token.TokenOutputModel
import pt.isel.markettracker.http.service.MarketTrackerService
import java.net.URL

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
        Log.v("TokenService", "createToken")
        val user =
            dummyUsers.find { it.email == input.email && it.password == input.password }
        Log.v("TokenService", user.toString())
        if (user != null) {
            val newToken = "token"
            dummyTokens.add(
                Token(
                    tokenValue = newToken,
                    userId = user.id
                )
            )
            Log.v("TokenService", "Token created")
            // obtain FCM token after successful login so that it can be uploaded to the server
            FirebaseMessaging.getInstance().token.addOnCompleteListener {
                if (!it.isSuccessful) {
                    return@addOnCompleteListener
                }
                val token = it.result
                Log.v("TokenService", token) // TODO: upload token to server
            }
            return TokenOutputModel(newToken)
        } else {
            return TokenOutputModel("")
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
        dummyTokens.removeIf { it.tokenValue == tokenValue }
    }
}