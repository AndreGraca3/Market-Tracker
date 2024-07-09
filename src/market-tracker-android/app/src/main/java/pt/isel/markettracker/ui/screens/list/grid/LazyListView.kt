package pt.isel.markettracker.ui.screens.list.grid

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.clickable
import androidx.compose.foundation.combinedClickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.LocalTextStyle
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.screens.list.cards.ListCard
import pt.isel.markettracker.ui.screens.list.components.ListIconButtons
import pt.isel.markettracker.ui.screens.list.components.ListNameDisplay
import pt.isel.markettracker.ui.screens.list.components.ListStatusIcons

@OptIn(ExperimentalFoundationApi::class)
@Composable
fun LazyListView(
    lists: List<ShoppingList>,
    isCreatingNewList: Boolean,
    isLoading: Boolean,
    newListName: String,
    onNewListNameChangeRequested: (String) -> Unit,
    onCreateListRequested: () -> Unit,
    onCancelRequested: () -> Unit,
    onLongClickRequest: (ShoppingList) -> Unit,
    onListItemClick: (String) -> Unit,
) {
    Column(
        verticalArrangement = Arrangement.Top,
        horizontalAlignment = Alignment.CenterHorizontally,
        modifier = Modifier.fillMaxSize()
    ) {
        if (isCreatingNewList && lists.size < R.integer.max_lists_per_user) {
            Box(
                contentAlignment = Alignment.Center,
                modifier = Modifier
                    .size(width = 330.dp, 125.dp)
            ) {
                ListCard(
                    isLoading = isLoading,
                    listNameContent = {
                        MarketTrackerTextField(
                            value = newListName,
                            onValueChange = {
                                onNewListNameChangeRequested(it)
                            },
                            textStyle = LocalTextStyle.current.copy(textAlign = TextAlign.Center),
                        )
                    },
                    listIconsContent = {
                        ListIconButtons(
                            onCreateListRequested = onCreateListRequested,
                            onCancelRequested = onCancelRequested
                        )
                    },
                    loadingContent = { LoadingIcon(text = "Criando a lista...") }
                )
            }
        }

        LazyColumn(
            verticalArrangement = Arrangement.spacedBy(10.dp),
            contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
        ) {
            items(lists.size) { index ->
                val currList = lists[index]
                ListCard(
                    isLoading = false,
                    listNameContent = {
                        ListNameDisplay(
                            lists[index].name
                        )
                    },
                    listIconsContent = {
                        ListStatusIcons(
                            isOwner = currList.isOwner,
                            numberOfParticipants = currList.numberOfMembers
                        )
                    },
                    loadingContent = {},
                    modifier = Modifier
                        .clickable { }
                        .combinedClickable(
                            onClick = { onListItemClick(currList.id) },
                            onLongClick = { onLongClickRequest(currList) }
                        )
                )
            }
        }
    }
}