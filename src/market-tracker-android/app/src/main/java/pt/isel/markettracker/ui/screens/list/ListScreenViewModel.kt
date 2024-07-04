package pt.isel.markettracker.ui.screens.list

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

@HiltViewModel
class ListScreenViewModel @Inject constructor(
    private val listService: IListService,
) : ViewModel() {
    private val _listsInfoFlow: MutableStateFlow<ShoppingListsScreenState> =
        MutableStateFlow(ShoppingListsScreenState.Idle)

    val listsInfo
        get() = _listsInfoFlow

    var isEditing by mutableStateOf(false)

    var listName by mutableStateOf("")

    /** Id of the list currently selected **/
    var idList by mutableStateOf<String?>(null)

    fun fetchLists(forceRefresh: Boolean = false) {
        if (_listsInfoFlow.value !is ShoppingListsScreenState.Idle && !forceRefresh) return

        _listsInfoFlow.value = ShoppingListsScreenState.Loading
        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.getLists()
            }.onSuccess {
                _listsInfoFlow.value = ShoppingListsScreenState.Loaded(it)
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }

    fun addList() {
        if (_listsInfoFlow.value !is ShoppingListsScreenState.Loaded
            || _listsInfoFlow.value.extractShoppingLists().size == 10
        ) return

        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.addList(listName)
                listService.getLists()
            }.onSuccess {
                _listsInfoFlow.value = ShoppingListsScreenState.Loaded(it)
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }

    fun deleteList() {
        if (_listsInfoFlow.value !is ShoppingListsScreenState.Loaded || idList.isNullOrBlank()) return

        _listsInfoFlow.value = ShoppingListsScreenState.Loading
        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.deleteListById(idList!!)
            }.onSuccess {
                _listsInfoFlow.value = ShoppingListsScreenState.Loaded(
                    _listsInfoFlow.value.extractShoppingLists().filter { it.id != idList }
                )
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }

    fun archiveList() {
        if (_listsInfoFlow.value !is ShoppingListsScreenState.Loaded || idList.isNullOrBlank()) return

        _listsInfoFlow.value = ShoppingListsScreenState.Loading
        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.updateList(id = idList!!, listName = null, isArchived = true)
            }.onSuccess {
                _listsInfoFlow.value = ShoppingListsScreenState.Loaded(
                    _listsInfoFlow.value.extractShoppingLists().filter { it.id != idList }
                )
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }
}