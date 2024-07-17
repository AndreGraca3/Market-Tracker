package pt.isel.markettracker.ui.screens.list

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.repository.auth.extractLists
import java.time.LocalDateTime
import javax.inject.Inject

@HiltViewModel
class ListScreenViewModel @Inject constructor(
    private val listService: IListService,
    private val authRepository: IAuthRepository
) : ViewModel() {
    private val _listsInfoFlow: MutableStateFlow<ShoppingListsScreenState> =
        MutableStateFlow(ShoppingListsScreenState.Idle)

    val listsInfoState
        get() = _listsInfoFlow

    var isCreatingNewList by mutableStateOf(false)
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
        val loadedState = _listsInfoFlow.value
        if (loadedState !is ShoppingListsScreenState.Loaded
            || _listsInfoFlow.value.extractShoppingLists().size == R.integer.max_lists_per_user
        ) return

        _listsInfoFlow.value =
            ShoppingListsScreenState.WaitFinishEditing(loadedState.shoppingLists)
        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.addList(listName)
            }.onSuccess {
                authRepository.addList(
                    ShoppingList(
                        it, listName, null, LocalDateTime.now(),
                        "", true, false, 1)
                )
                listName = ""
                isCreatingNewList = false
                _listsInfoFlow.value =
                    ShoppingListsScreenState.Loaded(authRepository.authState.value.extractLists())
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }

    fun deleteList() {
        val editState = _listsInfoFlow.value
        if (editState !is ShoppingListsScreenState.Editing) return
        val oldShoppingLists = editState.shoppingLists.toMutableList()

        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.deleteListById(editState.currentListEditing.id)
            }.onSuccess {
                oldShoppingLists
                    .removeIf { it.id == editState.currentListEditing.id }
                _listsInfoFlow.value = ShoppingListsScreenState.Loaded(oldShoppingLists)
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }

    fun archiveList() {
        val editState = _listsInfoFlow.value
        if (editState !is ShoppingListsScreenState.Editing) return
        val currentListSelected = editState.currentListEditing

        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.updateList(
                    id = currentListSelected.id,
                    listName = null,
                    isArchived = !currentListSelected.isArchived
                )
            }.onSuccess { updatedList ->
                _listsInfoFlow.value = ShoppingListsScreenState.Loaded(editState.shoppingLists.map {
                    if (it.id == updatedList.id) {
                        it.copy(
                            archivedAt = updatedList.archivedAt,
                            isArchived = updatedList.isArchived
                        )
                    } else it
                })
            }.onFailure {
                _listsInfoFlow.value = ShoppingListsScreenState.Failed(it)
            }
        }
    }

    fun stateToEditing(currentListSelected: ShoppingList) {
        val loadedState = _listsInfoFlow.value
        if (loadedState !is ShoppingListsScreenState.Loaded) return
        val currentList = loadedState.extractShoppingLists()

        _listsInfoFlow.value = ShoppingListsScreenState.Editing(currentList, currentListSelected)
    }

    fun resetToLoaded() {
        val editingState = _listsInfoFlow.value
        if (editingState !is ShoppingListsScreenState.Editing) return

        _listsInfoFlow.value = ShoppingListsScreenState.Loaded(editingState.shoppingLists)
    }
}