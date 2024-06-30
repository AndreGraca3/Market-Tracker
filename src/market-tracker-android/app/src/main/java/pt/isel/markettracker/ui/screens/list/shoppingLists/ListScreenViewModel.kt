package pt.isel.markettracker.ui.screens.list.shoppingLists

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

    var listName by mutableStateOf("")

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
        if (_listsInfoFlow.value !is ShoppingListsScreenState.Idle) return

        _listsInfoFlow.value = ShoppingListsScreenState.Loading
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

    fun deleteList(id: String) {
        if (_listsInfoFlow.value !is ShoppingListsScreenState.Idle) return

        _listsInfoFlow.value = ShoppingListsScreenState.Loading
        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.deleteListById(id)
            }.onSuccess {
                _listsInfoFlow.value = ShoppingListsScreenState.Loaded(
                    _listsInfoFlow.value.extractShoppingLists().filter { it.id != id }
                )
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }
}