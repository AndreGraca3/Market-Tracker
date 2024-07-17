package pt.isel.markettracker.ui.screens.users

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun UsersScreen(
    listId: String,
    usersScreenViewModel: UsersScreenViewModel = hiltViewModel(),
    onBackRequested: () -> Unit,
) {
    val usersState by usersScreenViewModel.stateFlow.collectAsState()

    LaunchedEffect(Unit) {
        usersScreenViewModel.fetchUsers()
    }

    UsersScreenView(
        state = usersState,
        query = usersScreenViewModel.usernameQuerySearch,
        onQueryChangeRequested = {
            usersScreenViewModel.usernameQuerySearch = it
        },
        onFetchUsersRequested = {
            usersScreenViewModel.fetchUsers(true)
        },
        onFetchMoreUsersRequested = usersScreenViewModel::loadMoreUsers,
        onAddUserToListRequested = { userId ->
            usersScreenViewModel.addUserToList(listId, userId)
        },
        onBackRequested = onBackRequested,
    )
}