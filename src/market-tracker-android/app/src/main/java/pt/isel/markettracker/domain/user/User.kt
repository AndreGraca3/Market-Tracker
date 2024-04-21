package pt.isel.markettracker.domain.user

data class User(
    val id: String,
    val username: String,
    val name: String,
    val email: String,
    val password: String,
    var avatar: String? = null,
    val createdAt: String = "2024-03-05T11:25:32.000Z"
)