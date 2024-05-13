package pt.isel.markettracker.http.models

import java.time.LocalDateTime

data class UserInfo(
    val id: Int,
    val name: String,
    val nickName: String,
    val avatarUrl: String?,
    val createdAt: LocalDateTime,
)

data class UserDetails(
    val id: Int,
    val email: String,
    val name: String,
    val nickName: String,
    val avatarUrl: String?,
    val createdAt: LocalDateTime,
)

data class UserItem(
    val id: Int,
    val nickName: String
)