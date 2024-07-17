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
import androidx.compose.material3.Tab
import androidx.compose.material3.TabRow
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.list.buttons.AddListButton
import pt.isel.markettracker.ui.screens.list.grid.ListGrid
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ListScreenView(
    state: ShoppingListsScreenState,
    newListName: String,
    isCreatingNewList: Boolean,
    fetchLists: (Boolean) -> Unit,
    onNewListNameChangeRequested: (String) -> Unit,
    onCreateListRequested: () -> Unit,
    onCancelCreatingListRequested: () -> Unit,
    onArchiveListRequested: () -> Unit,
    onEditListNameRequested: () -> Unit = {},
    onDeleteListRequested: () -> Unit,
    onListDetailsRequested: (String) -> Unit,
    onEditRequested: () -> Unit,
    onLongClickRequested: (ShoppingList) -> Unit,
    onDismissDialogRequested: () -> Unit,
) {
    var isRefreshing by remember { mutableStateOf(false) }

    var tabIndex by remember { mutableIntStateOf(0) }
    val tabs = listOf("Ativas", "Arquivadas")

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
                        enabled = tabIndex == 0,
                        onAddListRequested = onEditRequested,
                        modifier = Modifier.align(alignment = Alignment.CenterEnd)
                    )
                }
            }
        }
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
        ) {
            Box(
                modifier = Modifier.fillMaxWidth(),
                contentAlignment = Alignment.TopCenter
            ) {
                TabRow(selectedTabIndex = tabIndex) {
                    tabs.forEachIndexed { index, title ->
                        Tab(
                            text = {
                                Text(
                                    text = title,
                                    modifier = Modifier.align(Alignment.Center),
                                    textAlign = TextAlign.Center,
                                    fontFamily = mainFont
                                )
                            },
                            selected = tabIndex == index,
                            onClick = { tabIndex = index }
                        )
                    }
                }
            }

            Box {
                PullToRefreshLazyColumn(
                    isRefreshing = isRefreshing,
                    onRefresh = {
                        isRefreshing = true
                        fetchLists(true)
                    },
                ) {
                    Column(
                        modifier = Modifier
                            .fillMaxSize(),
                        horizontalAlignment = Alignment.CenterHorizontally,
                    ) {
                        ListGrid(
                            state = state,
                            tabIndex = tabIndex,
                            newListName = newListName,
                            onNewListNameChangeRequested = onNewListNameChangeRequested,
                            isCreatingNewList = isCreatingNewList,
                            onCreateListRequested = onCreateListRequested,
                            onCancelCreatingListRequested = onCancelCreatingListRequested,
                            onArchiveListRequest = onArchiveListRequested,
                            onDeleteListRequest = onDeleteListRequested,
                            onEditListNameRequested = onEditListNameRequested,
                            onListDetailsRequest = onListDetailsRequested,
                            onLongClickRequested = onLongClickRequested,
                            onDismissDialogRequest = onDismissDialogRequested,
                        )
                    }
                }
            }
        }
    }
}