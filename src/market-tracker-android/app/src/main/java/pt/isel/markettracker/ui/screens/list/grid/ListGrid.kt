package pt.isel.markettracker.ui.screens.list.grid

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ListAlt
import androidx.compose.material.icons.filled.Archive
import androidx.compose.material.icons.filled.Edit
import androidx.compose.material.icons.filled.RemoveShoppingCart
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.ui.components.buttons.MarketTrackerOutlinedButton
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.components.dialogs.MarketTrackerDialog
import pt.isel.markettracker.ui.screens.list.ShoppingListsScreenState
import pt.isel.markettracker.ui.screens.list.extractShoppingLists

@Composable
fun ListGrid(
    state: ShoppingListsScreenState,
    tabIndex: Int,
    newListName: String,
    isCreatingNewList: Boolean,
    onNewListNameChangeRequested: (String) -> Unit,
    onCreateListRequested: () -> Unit,
    onCancelCreatingListRequested: () -> Unit,
    onArchiveListRequest: () -> Unit,
    onDeleteListRequest: () -> Unit,
    onEditListNameRequested: () -> Unit,
    onListDetailsRequest: (String) -> Unit,
    onLongClickRequested: (ShoppingList) -> Unit,
    onDismissDialogRequest: () -> Unit,
) {
    val lists = state.extractShoppingLists()

    var openDialog by remember { mutableStateOf(false) }

    if (openDialog) {
        MarketTrackerDialog(
            icon = Icons.AutoMirrored.Filled.ListAlt,
            message = if (state !is ShoppingListsScreenState.Editing)
                "Tem de selecionar uma lista" else "O que pretende fazer à lista \" ${state.currentListEditing.name}\"?",
            onDismissRequest = {
                if (state is ShoppingListsScreenState.WaitFinishEditing) return@MarketTrackerDialog
                onDismissDialogRequest()
                openDialog = false
            }
        ) {
            when (state) {
                is ShoppingListsScreenState.Editing -> {
                    Column(
                        horizontalAlignment = Alignment.CenterHorizontally,
                        verticalArrangement = Arrangement.Center,
                        modifier = Modifier
                            .fillMaxSize()
                            .padding(vertical = 2.dp)
                    ) {
                        if (!state.currentListEditing.isArchived) {
                            MarketTrackerOutlinedButton(
                                text = "Arquivar",
                                icon = Icons.Default.Archive,
                                onClick = {
                                    onArchiveListRequest()
                                    openDialog = false
                                },
                                modifier = Modifier.padding(horizontal = 10.dp)
                            )
                        }
                        MarketTrackerOutlinedButton(
                            text = "Editar",
                            icon = Icons.Default.Edit,
                            onClick = onEditListNameRequested,
                            modifier = Modifier.padding(horizontal = 10.dp)
                        )
                        MarketTrackerOutlinedButton(
                            text = "Apagar",
                            icon = Icons.Default.RemoveShoppingCart,
                            onClick = {
                                onDeleteListRequest()
                                openDialog = false
                            },
                            modifier = Modifier.padding(horizontal = 10.dp)
                        )
                    }
                }

                is ShoppingListsScreenState.WaitFinishEditing -> {
                    LoadingIcon()
                }

                else -> {}
            }
        }
    }

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier.fillMaxSize()
    ) {
        when (state) {
            is ShoppingListsScreenState.Loaded,
            is ShoppingListsScreenState.Editing,
            is ShoppingListsScreenState.WaitFinishEditing,
            -> {
                val activeLists = lists.filter { !it.isArchived }
                val archivedLists = lists.filter { it.isArchived }
                LazyListView(
                    lists = when (tabIndex) {
                        0 -> activeLists
                        1 -> archivedLists
                        else -> emptyList()
                    },
                    isCreatingNewList = isCreatingNewList,
                    isLoading = state is ShoppingListsScreenState.WaitFinishEditing,
                    newListName = newListName,
                    onNewListNameChangeRequested = onNewListNameChangeRequested,
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
                            painter = painterResource(id = R.drawable.products_not_found),
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