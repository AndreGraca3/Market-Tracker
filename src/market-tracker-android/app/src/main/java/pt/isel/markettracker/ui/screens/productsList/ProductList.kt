package pt.isel.markettracker.ui.screens.productsList

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
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.ui.components.common.IOResourceLoader

@Composable
fun ProductList(productListState: IOState<ShoppingListEntries>) {

    IOResourceLoader(resource = productListState, errorContent = {
        Text(text = "Error loading list items")
    }) { shoppingListEntries ->

        LazyColumn(
            verticalArrangement = Arrangement.spacedBy(10.dp),
            contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
        ) {
            val listItems = shoppingListEntries.entries

            if (listItems.isEmpty()) {
                item {
                    Text(
                        text = "Esta lista está vazia."
                    )
                }
            }
            items(listItems.size) { index ->
                Box(
                    contentAlignment = Alignment.Center,
                    modifier = Modifier
                        .size(width = 350.dp, 100.dp)
                ) {
                    ProductListCard(listItems[index])
                }
            }
        }
    }
}