package pt.isel.markettracker.http.service.operations.list.listEntry

import pt.isel.markettracker.domain.model.list.listEntry.ListEntry
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries

interface IListEntryService {

    suspend fun addListEntry(
        listId: String,
        productId: String,
        storeId: Int,
        quantity: Int,
    ): String

    suspend fun updateListEntry(
        listId: String,
        entryId: String,
        storeId: Int,
        quantity: Int,
    ): ListEntry

    suspend fun deleteListEntry(listId: String, entryId: String)

    suspend fun getListEntries(
        listId: String,
        alternativeType: AlternativeType? = null,
        companyIds: List<Int> = emptyList(),
        storeIds: List<Int> = emptyList(),
        cityIds: List<Int> = emptyList(),
    ): ShoppingListEntries
}