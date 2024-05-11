package pt.isel.markettracker.http.service.operations.list

import pt.isel.markettracker.domain.list.ListInfo

interface IListService {

    suspend fun getListsInfo(): List<ListInfo>

}