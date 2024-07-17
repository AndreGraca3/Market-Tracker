package pt.isel.markettracker.http.service.operations.list.listEntry

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.list.listEntry.ListEntry
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.models.list.ListEntryCreationInputModel
import pt.isel.markettracker.http.models.list.ListEntryUpdateInputModel
import pt.isel.markettracker.http.service.MarketTrackerService

private fun buildListEntryPath(
    listId: String,
    alternativeType: AlternativeType? = null,
    companyIds: List<Int> = emptyList(),
    storeIds: List<Int> = emptyList(),
    cityIds: List<Int> = emptyList(),
) =
    "/lists/$listId/entries" +
            if (alternativeType == null
                && companyIds.isEmpty()
                && storeIds.isEmpty()
                && cityIds.isEmpty()
            ) "" else "?" +
                    alternativeType?.let { "alternativeType=$it" } +
                    (if (companyIds.isNotEmpty()) companyIds.joinToString(
                        separator = "&brandIds=",
                        prefix = "&brandIds="
                    ) else "") +
                    (if (storeIds.isNotEmpty()) storeIds.joinToString(
                        separator = "&companyIds=",
                        prefix = "&companyIds="
                    ) else "") +
                    (if (cityIds.isNotEmpty()) cityIds.joinToString(
                        separator = "&categoryIds=",
                        prefix = "&categoryIds="
                    ) else "")

private fun buildListEntryByIdPath(listId: String, entryId: String) =
    "${buildListEntryPath(listId)}/$entryId"

class ListEntryService(
    override val httpClient: OkHttpClient,
    override val gson: Gson,
) : IListEntryService, MarketTrackerService() {
    override suspend fun addListEntry(
        listId: String,
        productId: String,
        storeId: Int,
        quantity: Int,
    ): String {
        return requestHandler<StringIdOutputModel>(
            path = buildListEntryPath(listId),
            method = HttpMethod.POST,
            body = ListEntryCreationInputModel(
                productId = productId,
                storeId = storeId,
                quantity = quantity
            )
        ).id
    }

    override suspend fun updateListEntry(
        listId: String,
        entryId: String,
        storeId: Int,
        quantity: Int,
    ): ListEntry {
        return requestHandler(
            path = buildListEntryByIdPath(listId, entryId),
            method = HttpMethod.PATCH,
            body = ListEntryUpdateInputModel(
                storeId = storeId,
                quantity = quantity
            )
        )
    }

    override suspend fun deleteListEntry(listId: String, entryId: String) {
        return requestHandler(
            path = buildListEntryByIdPath(listId, entryId),
            method = HttpMethod.DELETE
        )
    }

    override suspend fun getListEntries(
        listId: String,
        alternativeType: AlternativeType?,
        companyIds: List<Int>,
        storeIds: List<Int>,
        cityIds: List<Int>,
    ): ShoppingListEntries {
        return requestHandler(
            path = buildListEntryPath(listId, alternativeType, companyIds, storeIds, cityIds),
            method = HttpMethod.GET
        )
    }
}

enum class AlternativeType{
    Cheapest,
}
