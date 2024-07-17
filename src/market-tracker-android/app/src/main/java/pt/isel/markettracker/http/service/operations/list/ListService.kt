package pt.isel.markettracker.http.service.operations.list

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.list.ListCreationInputModel
import pt.isel.markettracker.http.models.list.UpdateListInputModel
import pt.isel.markettracker.http.service.MarketTrackerService
import java.time.LocalDateTime

private fun buildListsPath(
    listName: String? = null,
    createdAfter: LocalDateTime? = null,
    isArchived: Boolean? = null,
    isOwner: Boolean? = null,
) =
    "/lists" + if (
        listName == null
        && createdAfter == null
        && isArchived == null
        && isOwner != null
    ) "" else "?" +
            listName?.let { "listName=$it" }.orEmpty() +
            createdAfter?.let { "&createdAfter=$it" }.orEmpty() +
            isArchived?.let { "&isArchived=$it" }.orEmpty() +
            isOwner?.let { "isOwner=$it" }.orEmpty() // TODO: Don't forget about &

private fun buildListsByIdPath(id: String) = "/lists/$id"

private fun buildListsClientByIdsPath(
    id: String,
    clientId: String,
) = "${buildListsByIdPath(id)}/clients/$clientId"

class ListService(
    override val httpClient: OkHttpClient,
    override val gson: Gson,
) : IListService, MarketTrackerService() {
    override suspend fun getLists(
        listName: String?,
        createdAfter: LocalDateTime?,
        isArchived: Boolean?,
        isOwner: Boolean?,
    ): List<ShoppingList> {
        return requestHandler<CollectionOutputModel<ShoppingList>>(
            path = buildListsPath(listName, createdAfter, isArchived, isOwner),
            method = HttpMethod.GET
        ).items
    }

    override suspend fun getListById(id: String): ShoppingListSocial {
        return requestHandler(
            path = buildListsByIdPath(id),
            method = HttpMethod.GET
        )
    }

    override suspend fun addList(listName: String): String {
        return requestHandler<StringIdOutputModel>(
            path = buildListsPath(),
            method = HttpMethod.POST,
            body = ListCreationInputModel(
                listName = listName
            )
        ).value
    }

    override suspend fun updateList(
        id: String,
        listName: String?,
        isArchived: Boolean?,
    ): ShoppingList {
        return requestHandler(
            path = buildListsByIdPath(id),
            method = HttpMethod.PATCH,
            body = UpdateListInputModel(
                listName = listName,
                isArchived = isArchived
            )
        )
    }

    override suspend fun deleteListById(id: String) {
        return requestHandler(
            path = buildListsByIdPath(id),
            method = HttpMethod.DELETE
        )
    }

    override suspend fun addClientToList(id: String, clientId: String) {
        return requestHandler(
            path = buildListsClientByIdsPath(id, clientId),
            method = HttpMethod.PUT
        )
    }

    override suspend fun removeClientFromList(id: String, clientId: String) {
        return requestHandler(
            path = buildListsClientByIdsPath(id, clientId),
            method = HttpMethod.DELETE
        )
    }
}