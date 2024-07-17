package pt.isel.markettracker.ui.screens.list

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun ListScreen(
    onListItemClick: (String) -> Unit,
    listScreenViewModel: ListScreenViewModel = hiltViewModel(),
) {
    val listState by listScreenViewModel.listsInfoState.collectAsState()

    ListScreenView(
        state = listState,
        newListName = listScreenViewModel.listName,
        isCreatingNewList = listScreenViewModel.isCreatingNewList,
        fetchLists = { forceRefresh ->
            listScreenViewModel.fetchLists(forceRefresh)
        },
        onNewListNameChangeRequested = {
            listScreenViewModel.listName = it
        },
        onCreateListRequested = listScreenViewModel::addList,
        onCancelCreatingListRequested = {
            listScreenViewModel.listName = ""
            listScreenViewModel.isCreatingNewList = false
        },
        onArchiveListRequested = listScreenViewModel::archiveList,
        onDeleteListRequested = listScreenViewModel::deleteList,
        onListDetailsRequested = onListItemClick,
        onEditRequested = {
            listScreenViewModel.isCreatingNewList = !listScreenViewModel.isCreatingNewList
        },
        onLongClickRequested = {
            listScreenViewModel.stateToEditing(it)
        },
        onDismissDialogRequested = {
            listScreenViewModel.resetToLoaded()
        }
    )
}