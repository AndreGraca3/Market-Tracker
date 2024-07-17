package pt.isel.markettracker.ui.screens.listDetails

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.operations.list.listEntry.IListEntryService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

@HiltViewModel
class ListDetailsScreenViewModel @Inject constructor(
    private val listEntryService: IListEntryService,
    private val listService: IListService,
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
            }.onSuccess { shoppingListEntries ->
                _listDetailsFlow.value = ListDetailsScreenState.PartiallyLoaded(shoppingListEntries)
                viewModelScope.launch {
                    runCatchingAPIFailure {
                        listService.getListById(id = listId)
                    }.onSuccess { shoppingListSocial ->
                        _listDetailsFlow.value =
                            ListDetailsScreenState.Loaded(
                                shoppingListEntries,
                                shoppingListSocial
                            )
                    }.onFailure {
                        _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
                    }
                }
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }

    fun changeProductCount(listId: String, entryId: String, storeId: Int, newQuantity: Int) {
        val successState = _listDetailsFlow.value
        if (successState !is ListDetailsScreenState.Loaded
            || successState.shoppingListEntries.entries.find { it.id == entryId } == null
        ) return

        _listDetailsFlow.value = ListDetailsScreenState.Editing(
            successState.shoppingListEntries,
            successState.shoppingListSocial
        )
        viewModelScope.launch {
            runCatchingAPIFailure {
                listEntryService.updateListEntry(
                    listId = listId,
                    entryId = entryId,
                    storeId = storeId,
                    quantity = newQuantity
                )
            }.onSuccess {
                val oldEntry = successState.shoppingListEntries.entries.find { it.id == entryId }!!
                _listDetailsFlow.value = ListDetailsScreenState.Loaded(
                    successState.shoppingListEntries.copy(
                        entries = successState.shoppingListEntries.entries.map {
                            if (it.id == entryId) {
                                it.copy(
                                    quantity = newQuantity
                                )
                            } else it
                        },
                        totalPrice =
                        successState.shoppingListEntries.totalPrice +
                                ((newQuantity - oldEntry.quantity) *
                                        oldEntry.productOffer.storeOffer.price.finalPrice)
                    ),
                    successState.shoppingListSocial
                )
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }

    fun deleteProductFromList(listId: String, entryId: String) {
        val successState = _listDetailsFlow.value
        val oldEntry = successState.extractShoppingListEntries().entries.find { it.id == entryId }
        if (successState !is ListDetailsScreenState.Loaded || oldEntry == null) return

        _listDetailsFlow.value = ListDetailsScreenState.WaitingForEditing(
            successState.shoppingListEntries,
            successState.shoppingListSocial,
            entryId
        )
        viewModelScope.launch {
            runCatchingAPIFailure {
                listEntryService.deleteListEntry(
                    listId = listId,
                    entryId = entryId
                )
            }.onSuccess {
                val oldList = successState.shoppingListEntries.entries.toMutableList()
                oldList.remove(oldEntry)
                _listDetailsFlow.value = ListDetailsScreenState.Loaded(
                    successState.shoppingListEntries.copy(
                        entries = oldList,
                        totalPrice =
                        successState.shoppingListEntries.totalPrice + (
                                oldEntry.quantity *
                                        oldEntry.productOffer.storeOffer.price.finalPrice),
                        totalProducts = successState.shoppingListEntries.totalProducts - 1
                    ),
                    successState.shoppingListSocial
                )
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }

    fun removeUserFromList(listId: String, userId: String) {
        val loadedState = _listDetailsFlow.value
        if (loadedState !is ListDetailsScreenState.Loaded) return
        val shoppingListEntries = loadedState.shoppingListEntries
        val oldShoppingListSocial = loadedState.shoppingListSocial

        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.removeClientFromList(id = listId, clientId = userId)
            }.onSuccess {
                val newMembers = oldShoppingListSocial.members.toMutableList()
                newMembers.removeIf { clientItem -> clientItem.id == userId }
                _listDetailsFlow.value = ListDetailsScreenState.Loaded(
                    shoppingListEntries,
                    oldShoppingListSocial.copy(
                        members = newMembers
                    )
                )
            }.onFailure {
                _listDetailsFlow.value = ListDetailsScreenState.Failed(it)
            }
        }
    }
}