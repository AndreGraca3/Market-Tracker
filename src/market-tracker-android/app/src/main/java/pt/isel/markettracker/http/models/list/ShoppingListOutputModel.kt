package pt.isel.markettracker.http.models.list

import java.time.LocalDateTime

data class ShoppingListOutputModel(
    val id: String,
    val name: String,
    val archivedAt: LocalDateTime?,
    val createdAt: LocalDateTime,
    val ownerId: String,
    val isOwner: Boolean,
    val isArchived: Boolean
)