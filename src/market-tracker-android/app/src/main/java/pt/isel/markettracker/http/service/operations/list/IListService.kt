package pt.isel.markettracker.http.service.operations.list

import pt.isel.markettracker.domain.model.market.list.ListInfo

interface IListService {

    suspend fun getListsInfo(): List<ListInfo>

    suspend fun deleteListById(id: Int)

}