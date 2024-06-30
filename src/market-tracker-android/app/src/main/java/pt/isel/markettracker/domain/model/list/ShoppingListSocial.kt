package pt.isel.markettracker.domain.model.list

import pt.isel.markettracker.domain.model.account.ClientItem
import java.time.LocalDateTime

data class ShoppingListSocial(
    val id: Int,
    val name: String,
    val archivedAt: LocalDateTime?,
    val createdAt: LocalDateTime,
    val ownerId: ClientItem,
    val members: List<ClientItem>,
)