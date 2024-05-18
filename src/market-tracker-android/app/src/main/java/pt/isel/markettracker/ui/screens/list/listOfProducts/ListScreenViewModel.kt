package pt.isel.markettracker.ui.screens.list.listOfProducts

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Idle
import pt.isel.markettracker.domain.fail
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.model.market.list.ListInfo
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.http.service.operations.list.IListService
import javax.inject.Inject

@HiltViewModel
class ListScreenViewModel @Inject constructor(
    private val listService: IListService
) : ViewModel() {
    companion object {
        const val MAX_GRID_COLUMNS = 1
    }

    // actual listed items
    private val listsInfoFlow: MutableStateFlow<IOState<List<ListInfo>>> =
        MutableStateFlow(idle())

    val listsInfo
        get() = listsInfoFlow

    fun fetchListInfo(forceRefresh: Boolean = false) {
        if (listsInfoFlow.value !is Idle && !forceRefresh) return

        listsInfoFlow.value =loading()
        viewModelScope.launch {
            val res = kotlin.runCatching {
                listService.getListsInfo()
            }
            listsInfoFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!) // TODO: handle error
            }
        }

    }

}