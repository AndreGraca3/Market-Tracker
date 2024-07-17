package pt.isel.markettracker.domain.model.list

import java.time.LocalDateTime

data class ShoppingList(
    val id: String,
    val name: String,
    val archivedAt: LocalDateTime?,
    val createdAt: LocalDateTime,
    val ownerId: String,
    val isOwner: Boolean,
    val isArchived: Boolean,
    val numberOfMembers: Int,
)