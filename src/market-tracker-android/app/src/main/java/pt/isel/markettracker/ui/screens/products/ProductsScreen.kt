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
        query = state.query,
        currentSearchTerm = productsScreenViewModel.currentSearchTerm,
        onCurrentSearchTermChange = { productsScreenViewModel.currentSearchTerm = it },
        fetchProducts = { query, forceRefresh ->
            productsScreenViewModel.fetchProducts(state.query, forceRefresh)
        },
        loadMoreProducts = { productsScreenViewModel.loadMoreProducts(it) },
        onProductClick = onProductClick,
        onBarcodeScanRequest = onBarcodeScanRequest
    )
}