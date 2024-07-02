package pt.isel.markettracker.ui.screens.list.grid

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.ui.screens.list.cards.ListCard
import pt.isel.markettracker.ui.screens.list.cards.ListItemCardEdit

@Composable
fun LazyListView(
    lists: List<ShoppingList>,
    isEditing: Boolean,
    value: String,
    onCreateListRequested: () -> Unit,
    onCancelRequested: () -> Unit,
    onLongClickRequest: (String) -> Unit,
    onListItemClick: (String) -> Unit,
) {
    Column(
        verticalArrangement = Arrangement.Top,
        horizontalAlignment = Alignment.CenterHorizontally,
        modifier = Modifier.fillMaxSize()
    ) {
        if (isEditing && lists.size < 10) {
            Box(
                contentAlignment = Alignment.Center,
                modifier = Modifier
                    .size(width = 330.dp, 125.dp)
            ) {
                ListItemCardEdit(
                    value = value,
                    onCreateListRequested = onCreateListRequested,
                    onCancelRequested = onCancelRequested
                )
            }
        }

        LazyColumn(
            verticalArrangement = Arrangement.spacedBy(10.dp),
            contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
        ) {
            items(lists.size) { index ->
                ListCard(
                    listInfo = lists[index],
                    onListItemClick = onListItemClick,
                    onLongClickRequest = onLongClickRequest
                )
            }
        }
    }
}