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
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.products.filters.FilterOptionsRow
import pt.isel.markettracker.ui.screens.products.grid.ProductsGridView
import pt.isel.markettracker.ui.screens.products.topbar.ProductsTopBar

@Composable
fun ProductsScreenView(
    state: ProductsScreenState,
    query: ProductsQuery,
    onQueryChange: (ProductsQuery) -> Unit,
    fetchProducts: (ProductsQuery, Boolean) -> Unit,
    loadMoreProducts: (ProductsQuery) -> Unit,
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit,
) {
    var isRefreshing by remember { mutableStateOf(false) }

    LaunchedEffect(Unit) {
        fetchProducts(query, false)
    }

    LaunchedEffect(state) {
        if (state !is ProductsScreenState.Loading) {
            isRefreshing = false // stop circular indicator
        }
    }

    Scaffold(
        topBar = {
            ProductsTopBar(
                searchQuery = query.searchTerm.orEmpty(),
                onSearchQueryChange = {
                    onQueryChange(query.copy(searchTerm = it))
                },
                onSearch = {
                    fetchProducts(query, true)
                },
                onBarcodeScanRequest = onBarcodeScanRequest
            )
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                isRefreshing = true
                fetchProducts(query, true)
            },
            modifier = Modifier
                .padding(paddingValues)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize(),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                FilterOptionsRow(
                    query = query,
                    onQueryChange = {
                        onQueryChange(it)
                        fetchProducts(it, true)
                    },
                    isLoading = state is ProductsScreenState.Loading
                )

                ProductsGridView(
                    state = state,
                    loadMoreProducts = { loadMoreProducts(query) },
                    onProductClick = onProductClick,
                )
            }
        }
    }
}