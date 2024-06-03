package pt.isel.markettracker.http.service.operations.list

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.dummy.dummyShoppingListEntries
import pt.isel.markettracker.dummy.dummyShoppingListSocial
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.service.MarketTrackerService

class ListService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IListService, MarketTrackerService(
) {

    override suspend fun getLists(): List<ShoppingListSocial> {
        delay(1000)
        return dummyShoppingListSocial
    }

    override suspend fun getListById(id: String): ShoppingListSocial {
        TODO("Not yet implemented")
    }

    override suspend fun addList(): StringIdOutputModel {
        TODO("Not yet implemented")
    }

    override suspend fun updateList(): ShoppingList {
        TODO("Not yet implemented")
    }

    override suspend fun deleteListById(id: Int) {
        delay(1000)
        dummyShoppingListSocial.removeIf { it.id == id }
    }

    override suspend fun addClientToList() {
        TODO("Not yet implemented")
    }

    override suspend fun removeClientFromList() {
        TODO("Not yet implemented")
    }
}