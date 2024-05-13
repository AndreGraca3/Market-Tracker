package pt.isel.markettracker.ui.screens.products

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun ProductsScreen(
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit,
    productsScreenViewModel: ProductsScreenViewModel = hiltViewModel()
) {
    val state by productsScreenViewModel.stateFlow.collectAsState()

    ProductsScreenView(
        state = state,
        query = productsScreenViewModel.query,
        onQueryChange = { productsScreenViewModel.query = it },
        fetchProducts = { query, forceRefresh ->
            productsScreenViewModel.fetchProducts(query, forceRefresh)
        },
        loadMoreProducts = { productsScreenViewModel.loadMoreProducts(it) },
        onProductClick = onProductClick,
        onBarcodeScanRequest = onBarcodeScanRequest
    )
}