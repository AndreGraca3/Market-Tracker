package pt.isel.markettracker.ui.screens.list

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.domain.Loading



@Composable
fun ListScreen(
    onListItemClick: (Int) -> Unit,
    listScreenViewModel: ListScreenViewModel
) {
    LaunchedEffect(Unit) {
        listScreenViewModel.fetchListInfo()
    }

    val scope = rememberCoroutineScope()
    var isRefreshing by remember { mutableStateOf(false) }

    val listState by listScreenViewModel.listsInfo.collectAsState()

    PullToRefreshLazyColumn(
        isRefreshing = isRefreshing,
        onRefresh = {
            scope.launch {
                isRefreshing = true
                listScreenViewModel.fetchListInfo(true)
                listScreenViewModel.listsInfo.collect {
                    if (it !is Loading) {
                        isRefreshing = false
                    }
                }
            }
        },
        modifier = Modifier
            .padding(2.dp) // TODO: Change this value
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize(),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            ListGrid(listState, onListItemClick)
        }
    }
}