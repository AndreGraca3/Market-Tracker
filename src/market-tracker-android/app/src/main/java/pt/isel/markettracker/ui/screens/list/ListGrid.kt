package pt.isel.markettracker.ui.screens.list

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.list.ListInfo
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.screens.products.card.ProductCard

@Composable
fun ListGrid(listState: IOState<List<ListInfo>>, onListItemClick: (Int) -> Unit) {
    IOResourceLoader(resource = listState, errorContent = {
        Text(text = "Error loading list items")
    }) { listItems ->
        LazyColumn(
            verticalArrangement = Arrangement.spacedBy(10.dp),
            contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
        ) {
            if (listItems.isEmpty()) {
                item {
                    Text(
                        text = "No list items found"
                    )
                }
            }
            items(listItems.size) { index ->
                Box(
                    contentAlignment = Alignment.Center,
                    modifier = Modifier
                        .size(width = 350.dp, 125.dp)
                ) {
                    ListItemCard(
                        listItem = listItems[index],
                        onListItemClick = onListItemClick
                    )
                }
            }
        }
    }
}