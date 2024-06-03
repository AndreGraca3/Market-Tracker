package pt.isel.markettracker.ui.screens.list.shoppingLists

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.fail
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

@HiltViewModel
class ListScreenViewModel @Inject constructor(
    private val listService: IListService
) : ViewModel() {
    private val listsInfoFlow: MutableStateFlow<ShoppingListsScreenState> =
        MutableStateFlow(ShoppingListsScreenState.Idle)

    val listsInfo
        get() = listsInfoFlow

    fun fetchLists(forceRefresh: Boolean = false) {
        if (listsInfoFlow.value !is ShoppingListsScreenState.Idle && !forceRefresh) return

        listsInfoFlow.value = ShoppingListsScreenState.Loading
        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                listService.getLists()
            }

            res.onSuccess {
                listsInfoFlow.value = ShoppingListsScreenState.Loaded(it)
            }

            res.onFailure {
                listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }

    //fun deleteList(position: Int) {
    //    val currentState = listsInfoFlow.value
    //    if (currentState !is ShoppingListsScreenState.Loading) return
//
    //    val listToDelete = currentState.value.getOrNull()?.get(position) ?: return
    //    val listIdToDelete = listToDelete.id
//
    //    listsInfoFlow.value = loading(currentState.value.getOrNull())
    //    viewModelScope.launch {
    //        kotlin.runCatching { listService.deleteListById(listIdToDelete) }
    //            .onSuccess {
    //                val updatedLists = currentState.value.getOrNull()?.toMutableList()?.apply {
    //                    removeAt(position)
    //                } ?: return@launch
    //                listsInfoFlow.value = loaded(Result.success(updatedLists))
    //            }
    //            .onFailure { exception ->
    //                listsInfoFlow.value = fail(exception)
    //            }
    //    }
    //}
}