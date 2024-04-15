package pt.isel.markettracker.http.models.user

import java.time.LocalDate
import java.util.UUID

data class UserOutputModel(
    val id: UUID,
    val username: String,
    val name: String,
    val createdAt: LocalDate
)