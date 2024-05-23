package pt.isel.markettracker.domain.model.account

import java.time.LocalDateTime

data class User(
    val id: String,
    val username: String,
    val name: String,
    val email: String,
    val password: String,
    var avatar: String? = null,
    val createdAt: LocalDateTime,
)