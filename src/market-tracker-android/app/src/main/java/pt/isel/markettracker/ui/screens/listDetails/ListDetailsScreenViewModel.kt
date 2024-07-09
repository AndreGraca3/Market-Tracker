package pt.isel.markettracker.ui.screens.listDetails

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.list.listEntry.IListEntryService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

@HiltViewModel
class ListDetailsScreenViewModel @Inject constructor(
    private val listEntryService: IListEntryService,
) : ViewModel() {

    private val _listDetailsFlow: MutableStateFlow<ListDetailsScreenState> =
        MutableStateFlow(ListDetailsScreenState.Idle)
    val listDetails
        get() = _listDetailsFlow.asStateFlow()

    fun fetchListDetails(listId: String, forceRefresh: Boolean = false) {
        if (_listDetailsFlow.value !is ListDetailsScreenState.Idle && !forceRefresh) return

        _listDetailsFlow.value = ListDetailsScreenState.Loading
        viewModelScope.launch {
            runCatchingAPIFailure {
                listEntryService.getListEntries(listId = listId)
            }.onSuccess {
                _listDetailsFlow.value = ListDetailsScreenState.Success(it)
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }

    /**
     * Increments product count by 1 (default).
     **/
    fun incrementProductCount(listId: String, entryId: String, storeId: Int, quantity: Int = 1) {
        if (_listDetailsFlow.value !is ListDetailsScreenState.Success) return

        val entry =
            (_listDetailsFlow.value as ListDetailsScreenState.Success).shoppingListEntries.entries.find {
                it.id == entryId
            }

        if (entry == null) return

        viewModelScope.launch {
            runCatchingAPIFailure {
                listEntryService.updateListEntry(
                    listId = listId,
                    entryId = entryId,
                    storeId = storeId,
                    quantity = entry.quantity + quantity
                )
                listEntryService.getListEntries(
                    listId = listId
                )
            }.onSuccess {
                _listDetailsFlow.value = ListDetailsScreenState.Success(it)
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }

    /**
     * decrements product count by 1 (default).
     **/
    fun decrementProductCount(listId: String, entryId: String, storeId: Int, quantity: Int = 1) {
        if (_listDetailsFlow.value !is ListDetailsScreenState.Success) return

        val entry =
            (_listDetailsFlow.value as ListDetailsScreenState.Success).shoppingListEntries.entries.find {
                it.id == entryId
            }

        if (entry == null) return

        viewModelScope.launch {
            runCatchingAPIFailure {
                listEntryService.updateListEntry(
                    listId = listId,
                    entryId = entryId,
                    storeId = storeId,
                    quantity = entry.quantity - quantity
                )
                listEntryService.getListEntries(
                    listId = listId
                )
            }.onSuccess {
                _listDetailsFlow.value = ListDetailsScreenState.Success(it)
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }

    fun deleteProductFromList(listId: String, entryId: String) {
        if (_listDetailsFlow.value !is ListDetailsScreenState.Success) return
        viewModelScope.launch {
            runCatchingAPIFailure {
                listEntryService.deleteListEntry(
                    listId = listId,
                    entryId = entryId
                )
                listEntryService.getListEntries(
                    listId = listId
                )
            }.onSuccess {
                _listDetailsFlow.value = ListDetailsScreenState.Success(it)
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }
}