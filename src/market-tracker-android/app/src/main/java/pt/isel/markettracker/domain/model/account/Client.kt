package pt.isel.markettracker.domain.model.account

import android.net.Uri
import java.time.LocalDateTime

data class Client(
    val id: String,
    val username: String,
    val email: String,
    val avatar: String?,
    val createdAt: LocalDateTime,
)

data class ClientItem(
    val id: String,
    val username: String,
    val avatar: String?
)