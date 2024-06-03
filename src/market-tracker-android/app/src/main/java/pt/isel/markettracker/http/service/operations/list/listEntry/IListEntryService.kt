package pt.isel.markettracker.http.service.operations.list.listEntry

import pt.isel.markettracker.domain.model.list.listEntry.ListEntry
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel

interface IListEntryService {

    suspend fun addListEntry(/* TODO: input model*/): StringIdOutputModel

    suspend fun updateListEntry(/* TODO: input model*/): ListEntry

    suspend fun deleteListEntry(/* TODO: input model*/)

    suspend fun getListEntries(/* TODO: input model*/): ShoppingListEntries
}