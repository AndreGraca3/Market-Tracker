package pt.isel.markettracker.http.service.operations.list

import pt.isel.markettracker.domain.model.list.ShoppingList
import java.time.LocalDateTime
import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel

interface IListService {

    suspend fun getLists(
        listName: String? = null,
        createdAfter: LocalDateTime? = null,
        isArchived: Boolean? = null,
        isOwner: Boolean? = null
    ): List<ShoppingList>

    suspend fun getListById(id: String): ShoppingListSocial

    suspend fun addList(/* TODO: input model*/): StringIdOutputModel

    suspend fun updateList(/* TODO: input model*/): ShoppingList

    suspend fun deleteListById(id: Int)

    suspend fun addClientToList(/* TODO: input model*/)

    suspend fun removeClientFromList(/* TODO: input model*/)
}