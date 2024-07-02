package pt.isel.markettracker.ui.screens.list

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.list.components.AddListButton
import pt.isel.markettracker.ui.screens.list.grid.ListGrid
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ListScreenView(
    state: ShoppingListsScreenState,
    value: String,
    isEditing: Boolean,
    fetchLists: (Boolean) -> Unit,
    onCreateListRequested: () -> Unit,
    onCancelCreatingListRequested: () -> Unit,
    onArchiveListRequested: (String) -> Unit,
    onDeleteListRequested: () -> Unit,
    onListDetailsRequested: (String) -> Unit,
    onEditRequested: () -> Unit,
    onLongClickRequested: (String) -> Unit
) {
    var isRefreshing by remember { mutableStateOf(false) }

    LaunchedEffect(Unit) {
        fetchLists(false)
    }

    LaunchedEffect(state) {
        if (state !is ShoppingListsScreenState.Loading) {
            isRefreshing = false
        }
    }

    Scaffold(
        topBar = {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Red)
                    .padding(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Box(
                    modifier = Modifier.fillMaxWidth()
                ) {
                    HeaderLogo(
                        modifier = Modifier
                            .align(alignment = Alignment.CenterStart)
                            .size(48.dp)
                    )
                    Text(
                        text = "Listas 📝",
                        color = Color.White,
                        fontFamily = mainFont,
                        fontSize = 30.sp,
                        modifier = Modifier
                            .align(alignment = Alignment.Center)
                    )
                    AddListButton(
                        onAddListRequested = onEditRequested,
                        modifier = Modifier.align(alignment = Alignment.CenterEnd)
                    )
                }
            }
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                isRefreshing = true
                fetchLists(true)
            },
            modifier = Modifier
                .padding(paddingValues)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize(),
                horizontalAlignment = Alignment.CenterHorizontally,
            ) {
                ListGrid(
                    state = state,
                    value = value,
                    isEditing = isEditing,
                    onCreateListRequested = onCreateListRequested,
                    onCancelCreatingListRequested = onCancelCreatingListRequested,
                    onArchiveListRequest = onArchiveListRequested,
                    onDeleteListRequest = onDeleteListRequested,
                    onListDetailsRequest = onListDetailsRequested,
                    onLongClickRequested = onLongClickRequested
                )
            }
        }
    }
}