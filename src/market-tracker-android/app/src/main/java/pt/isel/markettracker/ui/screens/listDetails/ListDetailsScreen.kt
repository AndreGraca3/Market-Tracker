package pt.isel.markettracker.ui.screens.listDetails

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.hilt.navigation.compose.hiltViewModel
import kotlinx.coroutines.launch

@Composable
fun ListDetailsScreen(
    listId: String,
    onAddUsersToListRequested: () -> Unit,
    listDetailsScreenViewModel: ListDetailsScreenViewModel = hiltViewModel(),
) {
    val listEntriesState by listDetailsScreenViewModel.listDetails.collectAsState()

    var isRefreshing by remember { mutableStateOf(false) }
    val scope = rememberCoroutineScope()

    LaunchedEffect(listEntriesState) {
        if (listEntriesState !is ListDetailsScreenState.Loading && isRefreshing) {
            isRefreshing = false // stop circular indicator
        }
    }

    LaunchedEffect(Unit) {
        listDetailsScreenViewModel.fetchListDetails(listId)
    }

    ListDetailsScreenView(
        state = listEntriesState,
        onAddUsersToListRequested = onAddUsersToListRequested,
        onRemoveUserFromLisTRequested = { userId ->
            listDetailsScreenViewModel.removeUserFromList(listId, userId)
        },
        isRefreshing = isRefreshing,
        fetchListDetails = {
            scope.launch {
                isRefreshing = true
                listDetailsScreenViewModel.fetchListDetails(listId, true)
                listDetailsScreenViewModel.listDetails.collect {
                    if (it !is ListDetailsScreenState.Loaded) {
                        isRefreshing = false
                    }
                }
            }
        },
        onGenerateCheapestList = {
            // TODO("")
        },
        changeProductCount = { entryId, storeId, newQuantity ->
            listDetailsScreenViewModel.changeProductCount(listId, entryId, storeId, newQuantity)
        },
        deleteProductFromList = { entryId ->
            listDetailsScreenViewModel.deleteProductFromList(listId, entryId)
        }
    )
}