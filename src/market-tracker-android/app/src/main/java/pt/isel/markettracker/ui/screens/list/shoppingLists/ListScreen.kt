package pt.isel.markettracker.ui.screens.list.shoppingLists

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun ListScreen(
    onListItemClick: (String) -> Unit,
    listScreenViewModel: ListScreenViewModel = hiltViewModel()
) {
    val listState by listScreenViewModel.listsInfo.collectAsState()

    ListScreenView(
        state = listState,
        fetchLists = { forceRefresh ->
            listScreenViewModel.fetchLists(forceRefresh)
        },
        onAddListRequested = {
            // :)
        },
        onArchiveListRequested = {
            //::listScreenViewModel.archiveList
        },
        onDeleteListRequested = {
            //::ListScreenViewModel.deleteList
        },
        onListDetailsRequested = onListItemClick
    )
}