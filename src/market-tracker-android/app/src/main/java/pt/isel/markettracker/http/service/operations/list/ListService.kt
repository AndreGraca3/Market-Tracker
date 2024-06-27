package pt.isel.markettracker.http.service.operations.list

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.http.service.MarketTrackerService
import java.time.LocalDateTime

private fun buildListsPath(
    listName: String?,
    createdAfter: LocalDateTime?,
    isArchived: Boolean?,
    isOwner: Boolean?
) = "/lists?" +
        listName?.let { "&listName=$it" }.orEmpty() +
        createdAfter?.let { "&createdAfter=$it" }.orEmpty() +
        isArchived?.let { "&isArchived=$it" }.orEmpty() +
        isOwner?.let { "isOwner=$it" }.orEmpty() // TODO: Don't forgor about &

class ListService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IListService, MarketTrackerService() {
    override suspend fun getLists(
        listName: String?,
        createdAfter: LocalDateTime?,
        isArchived: Boolean?,
        isOwner: Boolean?
    ): List<ShoppingList> {
        return requestHandler<CollectionOutputModel<ShoppingList>>(
            path = buildListsPath(listName, createdAfter, isArchived, isOwner),
            method = HttpMethod.GET
        ).items
    }
}