package pt.isel.markettracker.domain.model.market.list

import java.time.LocalDateTime
import java.util.UUID

data class ListInfo(
    val id: Int,
    val listName: String,
    val archivedAt: LocalDateTime?,
    val createdAt: LocalDateTime,
    val ownerId: UUID,
    val numberOfParticipants: Int = 0,
    val isOwner: Boolean = false
)