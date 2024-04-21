package pt.isel.markettracker.http.models.user

data class UserCreationInputModel(
    val name: String,
    val username: String,
    val email: String,
    val password: String
)