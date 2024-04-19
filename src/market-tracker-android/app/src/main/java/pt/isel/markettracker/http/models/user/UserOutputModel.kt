package pt.isel.markettracker.http.models.user

data class UserOutputModel(
    val id: String,  //UUID
    val username: String,
    val name: String,
    val email: String,
    val avatar: String? = null,
    val createdAt: String = "2024-03-05T11:25:32.000Z"
)