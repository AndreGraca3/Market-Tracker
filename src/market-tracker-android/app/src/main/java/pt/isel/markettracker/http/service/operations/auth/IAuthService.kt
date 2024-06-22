package pt.isel.markettracker.http.service.operations.auth

interface IAuthService {
    suspend fun googleSignIn(googleIdToken: String)

    suspend fun signIn(email: String, password: String)

    suspend fun signOut()
}