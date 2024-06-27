package pt.isel.markettracker.http.service.operations.auth

interface IAuthService {
    suspend fun googleSignIn(idToken: String)

    suspend fun signIn(email: String, password: String)

    suspend fun signOut()
}