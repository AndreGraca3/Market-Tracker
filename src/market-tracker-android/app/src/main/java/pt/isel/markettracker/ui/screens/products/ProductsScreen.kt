package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import pt.isel.markettracker.ui.components.PullToRefreshLazyColumn

@Composable
fun ProductsScreen() {
    val products = remember {
        (1..100).map { "Product $it" }
    }
    var isRefreshing by remember { mutableStateOf(false) }

    val scope = rememberCoroutineScope()

    PullToRefreshLazyColumn(
        items = products,
        content = { ProductItem(it) },
        isRefreshing = isRefreshing,
        onRefresh = {
            scope.launch {
                isRefreshing = true
                delay(3000L) // Simulated API call
                isRefreshing = false
            }
        }
    )
}

@Composable
fun ProductItem(product: String) {
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp)
    ) {
        Text(
            text = product,
            style = MaterialTheme.typography.bodyLarge
        )
    }
}