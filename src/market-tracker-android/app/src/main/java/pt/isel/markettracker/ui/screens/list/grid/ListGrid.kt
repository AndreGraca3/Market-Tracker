package pt.isel.markettracker.ui.screens.list.grid

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ListAlt
import androidx.compose.material3.Button
import androidx.compose.material3.Tab
import androidx.compose.material3.TabRow
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.components.dialogs.MarketTrackerDialog
import pt.isel.markettracker.ui.screens.list.ShoppingListsScreenState
import pt.isel.markettracker.ui.screens.list.extractShoppingLists
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ListGrid(
    state: ShoppingListsScreenState,
    value: String,
    isEditing: Boolean,
    onCreateListRequested: () -> Unit,
    onCancelCreatingListRequested: () -> Unit,
    onArchiveListRequest: () -> Unit,
    onDeleteListRequest: () -> Unit,
    onListDetailsRequest: (String) -> Unit,
    onLongClickRequested: (String) -> Unit,
) {
    val lists = state.extractShoppingLists()

    var openDialog by remember { mutableStateOf(false) }

    var tabIndex by remember { mutableIntStateOf(0) }
    val tabs = listOf("Ativas", "Arquivadas")

    if (openDialog) {
        MarketTrackerDialog(
            icon = Icons.Default.ListAlt,
            message = "O que pretende fazer á lista?",
            onDismissRequest = { openDialog = false }
        ) {
            Row(
                horizontalArrangement = Arrangement.Center,
                verticalAlignment = Alignment.CenterVertically,
                modifier = Modifier.fillMaxSize()
            ) {
                Button(onClick = onArchiveListRequest) {
                    Text("Arquivar")
                }

                Button(onClick = onDeleteListRequest) {
                    Text("Apagar")
                }
            }
        }
    }

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier.fillMaxSize()
    ) {
        when (state) {
            is ShoppingListsScreenState.Loaded -> {
                val activeLists = lists.filter { !it.isArchived }
                val archivedLists = lists.filter { it.isArchived }

                TabRow(selectedTabIndex = tabIndex) {
                    tabs.forEachIndexed { index, title ->
                        Tab(text = {
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

                LazyListView(
                    lists = when (tabIndex) {
                        0 -> activeLists
                        1 -> archivedLists
                        else -> emptyList()
                    },
                    isEditing = isEditing,
                    value = value,
                    onCreateListRequested = onCreateListRequested,
                    onCancelRequested = onCancelCreatingListRequested,
                    onLongClickRequest = {
                        onLongClickRequested(it)
                        openDialog = true
                    },
                    onListItemClick = onListDetailsRequest
                )
            }

            is ShoppingListsScreenState.Failed -> {
                LazyColumn(
                    modifier = Modifier.fillMaxSize(),
                    verticalArrangement = Arrangement.Center,
                    horizontalAlignment = Alignment.CenterHorizontally
                ) {
                    item {
                        Image(
                            painter = painterResource(id = R.drawable.server_error),
                            contentDescription = "No lists"
                        )
                    }
                }
            }

            else -> {
                LoadingIcon(stringResource(id = R.string.lists_loading))
            }
        }
    }
}