package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.ProductsQuery
import pt.isel.markettracker.ui.screens.ProductsScreenState
import pt.isel.markettracker.ui.screens.ProductsSortOption
import pt.isel.markettracker.ui.screens.products.filters.FilterOptions
import pt.isel.markettracker.ui.screens.products.topbar.ProductsTopBar

@Composable
fun ProductsView(
    state: ProductsScreenState,
    fetchProducts: (ProductsQuery, Boolean, Boolean) -> Unit,
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit,
) {
    var query by rememberSaveable { mutableStateOf(ProductsQuery()) }
    var isRefreshing by remember { mutableStateOf(false) }

    LaunchedEffect(Unit) {
        fetchProducts(query, true, true)
    }

    LaunchedEffect(state) {
        if (state !is ProductsScreenState.Loading) {
            isRefreshing = false
        }
    }

    Scaffold(
        topBar = {
            ProductsTopBar(
                onSearch = {
                    fetchProducts(query, true, true)
                },
                searchQuery = query.searchTerm.orEmpty(),
                onQueryChange = {
                    query = query.copy(searchTerm = it)
                },
                onBarcodeScanRequest = onBarcodeScanRequest
            )
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                isRefreshing = true
                fetchProducts(query, true, true)
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
                    selectedSort = query.sortOption,
                    onFiltersRequest = {},
                    onSortRequest = {
                        query = query.copy(sortOption = ProductsSortOption.valueOf(it))
                        fetchProducts(query, true, true)
                    }
                )

                ProductsGrid(
                    state = state,
                    fetchProducts = { fetchProducts(query, true, false) },
                    onProductClick = onProductClick
                )
            }
        }
    }
}