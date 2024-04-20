package pt.isel.markettracker.http.service.operations.token

import pt.isel.markettracker.http.models.token.TokenCreationInputModel
import pt.isel.markettracker.http.models.token.TokenOutputModel

interface ITokenService {

    suspend fun createToken(input: TokenCreationInputModel): TokenOutputModel

    suspend fun deleteToken(tokenValue: String)
}