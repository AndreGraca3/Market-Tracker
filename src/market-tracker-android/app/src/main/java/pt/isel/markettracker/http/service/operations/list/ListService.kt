package pt.isel.markettracker.http.service.operations.list

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.market.list.ListInfo
import pt.isel.markettracker.dummy.dummyList
import pt.isel.markettracker.http.service.MarketTrackerService

class ListService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IListService, MarketTrackerService(
) {

    override suspend fun getListsInfo(): List<ListInfo> {
        delay(1000)
        return dummyList
    }
}