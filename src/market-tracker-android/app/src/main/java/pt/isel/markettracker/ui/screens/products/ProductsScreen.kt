package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn

@Composable
fun ProductsScreen(productsScreenViewModel: ProductsScreenViewModel) {

    LaunchedEffect(Unit) {
        productsScreenViewModel.fetchProducts()
    }

    var isRefreshing by remember { mutableStateOf(false) }

    val productsState by productsScreenViewModel.products.collectAsState(initial = idle())

    val scope = rememberCoroutineScope()

    PullToRefreshLazyColumn(
        isRefreshing = isRefreshing,
        onRefresh = {
            scope.launch {
                isRefreshing = true
                productsScreenViewModel.fetchProducts()
                productsScreenViewModel.products.collect() {
                    if (it !is Loading) {
                        isRefreshing = false
                    }
                }
            }
        }
    ) {
        IOResourceLoader(resource = productsState, errorContent = {
            Text(text = "Error loading products")
        }) { products ->
            LazyVerticalGrid(columns = GridCells.Fixed(ProductsScreenViewModel.MAX_GRID_COLUMNS)) {
                items(products.size) { index ->
                    ProductCard(product = products[index])
                }
            }
        }
    }
}