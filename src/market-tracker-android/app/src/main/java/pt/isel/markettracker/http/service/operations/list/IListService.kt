package pt.isel.markettracker.http.service.operations.list

import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.http.models.list.ShoppingListOutputModel
import java.time.LocalDateTime

interface IListService {

    suspend fun getLists(
        listName: String? = null,
        createdAfter: LocalDateTime? = null,
        isArchived: Boolean? = null,
        isOwner: Boolean? = null
    ): CollectionOutputModel<ShoppingListOutputModel>
}