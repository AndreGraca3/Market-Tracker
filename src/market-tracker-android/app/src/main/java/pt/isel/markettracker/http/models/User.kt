package pt.isel.markettracker.http.models

data class UserCreationInputModel(
    val name: String,
    val username: String,
    val email: String,
    val password: String
)

data class UserInfo(
    val id: Int,
    val name: String,
    val nickName: String,
    val avatarUrl: String?,
    val createdAt: String,
)

data class UserDetails(
    val id: Int,
    val email: String,
    val name: String,
    val nickName: String,
    val avatarUrl: String?,
    val createdAt: String,
)

data class UserItem(
    val id: Int,
    val nickName: String
)