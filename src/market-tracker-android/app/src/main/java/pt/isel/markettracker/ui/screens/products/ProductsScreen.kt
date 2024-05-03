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

    ProductsView(
        state = state,
        fetchProducts = { query, forceRefresh, forceReset ->
            productsScreenViewModel.fetchProducts(query, forceRefresh, forceReset)
        },
        onProductClick = onProductClick,
        onBarcodeScanRequest = onBarcodeScanRequest
    )
}