package pt.isel.markettracker.ui.screens.listDetails

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

@Composable
fun ProductList(
    state: ListDetailsScreenState,
) {
    when (state) {
        is ListDetailsScreenState.Success -> {
            LazyColumn(
                verticalArrangement = Arrangement.spacedBy(10.dp),
                contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
            ) {
                val listItems = state.shoppingListEntries.entries

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

        else -> {}
    }
}