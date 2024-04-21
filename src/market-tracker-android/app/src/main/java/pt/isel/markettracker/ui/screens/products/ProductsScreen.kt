package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.products.filters.FilterOptions
import pt.isel.markettracker.ui.screens.products.topbar.ProductsTopBar

@Composable
fun ProductsScreen(
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit,
    productsScreenViewModel: ProductsScreenViewModel
) {
    LaunchedEffect(Unit) {
        productsScreenViewModel.fetchProducts()
    }

    val scope = rememberCoroutineScope()
    var isRefreshing by remember { mutableStateOf(false) }

    val productsState by productsScreenViewModel.products.collectAsState()
    val searchQuery by productsScreenViewModel.searchQuery.collectAsState()
    val selectedSort by productsScreenViewModel.sortOption.collectAsState()

    Scaffold(
        topBar = {
            ProductsTopBar(
                searchQuery = searchQuery,
                onQueryChange = productsScreenViewModel::onSearchQueryChange,
                onSearch = {
                    productsScreenViewModel.fetchProducts(true)
                },
                onBarcodeScanRequest = onBarcodeScanRequest
            )
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                scope.launch {
                    isRefreshing = true
                    productsScreenViewModel.fetchProducts(true)
                    productsScreenViewModel.products.collect {
                        if (it !is Loading) {
                            isRefreshing = false
                        }
                    }
                }
            },
            modifier = Modifier
                .padding(paddingValues)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize(),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                FilterOptions(
                    selectedSort = selectedSort,
                    onFiltersRequest = {},
                    onSortRequest = {
                        productsScreenViewModel.onSortOptionChange(it)
                        productsScreenViewModel.fetchProducts(true)
                    }
                )
                ProductsGrid(productsState, onProductClick)
            }
        }
    }
}