package pt.isel.markettracker.ui.screens.list.shoppingLists.grid

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ListAlt
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.components.dialogs.MarketTrackerDialog
import pt.isel.markettracker.ui.screens.list.shoppingLists.ShoppingListsScreenState
import pt.isel.markettracker.ui.screens.list.shoppingLists.extractShoppingLists

@Composable
fun ListGrid(
    state: ShoppingListsScreenState,
    onArchiveListRequest: (String) -> Unit,
    onDeleteListRequest: (String) -> Unit,
    onListDetailsRequest: (String) -> Unit
) {
    val lists = state.extractShoppingLists()

    var openDialog by remember { mutableStateOf(false) }

    if (openDialog) {
        MarketTrackerDialog(
            icon = Icons.Default.ListAlt,
            message = "O que pretende fazer á lista?",
            onDismissRequest = { openDialog = false }) {
            Row(
                horizontalArrangement = Arrangement.Center,
                verticalAlignment = Alignment.CenterVertically,
                modifier = Modifier.fillMaxSize()
            ) {
                Button(onClick = { /** TODO: "not yet implemented" **/ }) {
                    Text("Arquivar")
                }

                Button(onClick = { /** TODO: "not yet implemented" **/ }) {
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
                LazyListView(
                    lists = lists,
                    onLongClickRequest = { openDialog = true },
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