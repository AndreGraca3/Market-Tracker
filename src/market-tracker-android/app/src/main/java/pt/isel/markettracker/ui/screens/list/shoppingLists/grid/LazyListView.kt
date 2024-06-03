package pt.isel.markettracker.ui.screens.list.shoppingLists.grid

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.ui.screens.list.shoppingLists.card.ListItemCard


/** This crap ain't no view **/
@Composable
fun LazyListView(
    lists: List<ShoppingListSocial>,
    onLongClickRequest: () -> Unit,
    onListItemClick: (Int) -> Unit
) {
    LazyColumn(
        verticalArrangement = Arrangement.spacedBy(10.dp),
        contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
    ) {
        items(lists.size) { index ->
            Box(
                contentAlignment = Alignment.TopCenter,
                modifier = Modifier
                    .size(width = 350.dp, 125.dp)
            ) {
                ListItemCard(
                    listInfo = lists[index],
                    onListItemClick = onListItemClick,
                    onLongClickRequest = onLongClickRequest
                )
            }
        }
    }
}