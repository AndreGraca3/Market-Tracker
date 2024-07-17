package pt.isel.markettracker.ui.screens.users.states

sealed class AddUserToListState {
    data object Idle : AddUserToListState()

    data object AddingToList : AddUserToListState()

    data object Success: AddUserToListState()

    data class Failed(val error: Throwable) : AddUserToListState()
}