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
    val listState by listScreenViewModel.listsInfo.collectAsState()

    ListScreenView(
        state = listState,
        value = listScreenViewModel.listName,
        isEditing = listScreenViewModel.isEditing,
        fetchLists = { forceRefresh ->
            listScreenViewModel.fetchLists(forceRefresh)
        },
        onCreateListRequested = listScreenViewModel::addList,
        onCancelCreatingListRequested = {
            listScreenViewModel.listName = ""
            listScreenViewModel.isEditing = false
        },
        onArchiveListRequested = listScreenViewModel::archiveList,
        onDeleteListRequested = listScreenViewModel::deleteList,
        onListDetailsRequested = onListItemClick,
        onEditRequested = {
            listScreenViewModel.isEditing = !listScreenViewModel.isEditing
        },
        onLongClickRequested = {
            listScreenViewModel.idList = it
        }
    )
}