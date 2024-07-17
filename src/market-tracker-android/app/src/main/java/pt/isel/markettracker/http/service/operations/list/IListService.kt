package pt.isel.markettracker.http.service.operations.list

import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import java.time.LocalDateTime

interface IListService {

    suspend fun getLists(
        listName: String? = null,
        createdAfter: LocalDateTime? = null,
        isArchived: Boolean? = null,
        isOwner: Boolean? = null,
    ): List<ShoppingList>

    suspend fun getListById(id: String): ShoppingListSocial

    suspend fun addList(listName: String): String

    suspend fun updateList(id: String, listName: String?, isArchived: Boolean?): ShoppingList

    suspend fun deleteListById(id: String)

    suspend fun addClientToList(id: String, clientId: String)

    suspend fun removeClientFromList(id: String, clientId: String)
}