package pt.isel.markettracker.domain.model.account

import java.time.LocalDateTime

data class Client(
    val id: String,
    val username: String,
    val email: String,
    var avatar: String?,
    val createdAt: LocalDateTime,
)

data class ClientItem(
    val id: String,
    val username: String,
    val avatar: String?
)