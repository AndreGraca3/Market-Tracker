package pt.isel.markettracker.http.service.operations.auth

import pt.isel.markettracker.http.models.token.GoogleTokenCreationInputModel
import pt.isel.markettracker.http.models.token.TokenCreationInputModel

interface IAuthService {
    suspend fun googleSignIn(input: GoogleTokenCreationInputModel)

    suspend fun signIn(input: TokenCreationInputModel)

    suspend fun signOut()
}