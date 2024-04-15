package pt.isel.markettracker.domain.user

data class User(
    val username: String,
    val name: String,
    val email: String,
    val password: String,
    val avatar: String
)