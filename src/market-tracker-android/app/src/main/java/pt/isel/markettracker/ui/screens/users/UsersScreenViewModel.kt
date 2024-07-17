package pt.isel.markettracker.ui.screens.users

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.ui.screens.users.states.AddUserToListState
import pt.isel.markettracker.ui.screens.users.states.UsersScreenState
import pt.isel.markettracker.ui.screens.users.states.extractUsers
import javax.inject.Inject

@HiltViewModel
class UsersScreenViewModel @Inject constructor(
    private val userService: IUserService,
    private val listService: IListService,
) : ViewModel() {
    companion object {
        const val MAX_GRID_COLUMNS = 1
    }

    private val _stateFlow: MutableStateFlow<UsersScreenState> =
        MutableStateFlow(UsersScreenState.Idle)
    val stateFlow
        get() = _stateFlow.asStateFlow()

    var usernameQuerySearch by mutableStateOf("")
    private var currentPage by mutableIntStateOf(1)

    fun fetchUsers(forceRefresh: Boolean = false) {
        if (_stateFlow.value !is UsersScreenState.Idle && !forceRefresh) return

        currentPage = 1
        _stateFlow.value = UsersScreenState.Loading

        handleUsersFetch()
    }

    fun loadMoreUsers() {
        val currentState = _stateFlow.value
        if (currentState is UsersScreenState.LoadingMore ||
            currentState !is UsersScreenState.Loaded ||
            !currentState.hasMore
        ) return

        _stateFlow.value = UsersScreenState.LoadingMore(currentState.users)
        handleUsersFetch()
    }

    private fun handleUsersFetch() {
        val oldUsers = _stateFlow.value.extractUsers()

        viewModelScope.launch {
            runCatchingAPIFailure {
                userService.getUsers(
                    page = currentPage++,
                    username = usernameQuerySearch
                )
            }.onSuccess {
                val allProducts = oldUsers + it.items
                _stateFlow.value =
                    UsersScreenState.IdleLoaded(allProducts, it.hasMore)
            }.onFailure {
                _stateFlow.value = UsersScreenState.Failed(it)
            }
        }
    }

    private val _addUserToListStateFlow: MutableStateFlow<AddUserToListState> =
        MutableStateFlow(AddUserToListState.Idle)
    val addUserToListStateFlow
        get() = _addUserToListStateFlow.asStateFlow()

    fun addUserToList(listId: String, userId: String) {
        if (addUserToListStateFlow.value is AddUserToListState.AddingToList) return

        viewModelScope.launch {
            runCatchingAPIFailure {
                listService.addClientToList(
                    id = listId,
                    userId
                )
            }.onSuccess {
                _addUserToListStateFlow.value = AddUserToListState.Success
            }.onFailure {
                _addUserToListStateFlow.value = AddUserToListState.Failed(it)
            }
        }
    }
}