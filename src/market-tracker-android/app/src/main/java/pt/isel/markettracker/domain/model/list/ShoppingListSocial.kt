package pt.isel.markettracker.domain.model.list

import pt.isel.markettracker.domain.model.account.ClientItem
import java.time.LocalDateTime

data class ShoppingListSocial(
    val id: String,
    val name: String,
    val archivedAt: LocalDateTime?,
    val createdAt: LocalDateTime,
    val owner: ClientItem,
    val members: List<ClientItem>,
)