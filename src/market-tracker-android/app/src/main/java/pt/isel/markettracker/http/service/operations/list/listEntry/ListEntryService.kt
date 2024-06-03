package pt.isel.markettracker.http.service.operations.list.listEntry

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.list.listEntry.ListEntry
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.dummy.dummyShoppingListEntries
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.service.MarketTrackerService

class ListEntryService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IListEntryService, MarketTrackerService() {
    override suspend fun addListEntry(): StringIdOutputModel {
        TODO("Not yet implemented")
    }

    override suspend fun updateListEntry(): ListEntry {
        TODO("Not yet implemented")
    }

    override suspend fun deleteListEntry() {
        TODO("Not yet implemented")
    }

    override suspend fun getListEntries(): ShoppingListEntries {
        delay(1000)
        return dummyShoppingListEntries
    }
}
