package pt.isel.markettracker.domain.model.list

import java.time.LocalDateTime
import java.util.UUID

data class ShoppingList(
    val id: String,
    val name: String,
    val archivedAt: LocalDateTime?,
    val createdAt: LocalDateTime,
    val ownerId: UUID,
    val isOwner: Boolean,
    val isArchived: Boolean
)