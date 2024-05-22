package pt.isel.markettracker.ui.screens.list.listOfProducts

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Idle
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.fail
import pt.isel.markettracker.domain.getOrNull
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

    fun deleteListAt(position: Int) {
        val currentState = listsInfoFlow.value
        if (currentState !is Loaded) return

        val listToDelete = currentState.value.getOrNull()?.get(position) ?: return
        val listIdToDelete = listToDelete.id

        listsInfoFlow.value = loading(currentState.value.getOrNull())
        viewModelScope.launch {
            kotlin.runCatching { listService.deleteListById(listIdToDelete) }
                .onSuccess {
                    val updatedLists = currentState.value.getOrNull()?.toMutableList()?.apply {
                        removeAt(position)
                    } ?: return@launch
                    listsInfoFlow.value = loaded(Result.success(updatedLists))
                }
                .onFailure { exception ->
                    listsInfoFlow.value = fail(exception)
                }
        }
    }


}