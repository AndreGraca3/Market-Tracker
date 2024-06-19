package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.inventory.product.filter.resetAll
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.products.filters.FilterOptionsRow
import pt.isel.markettracker.ui.screens.products.grid.ProductsGridView
import pt.isel.markettracker.ui.screens.products.topbar.ProductsTopBar

@Composable
fun ProductsScreenView(
    state: ProductsScreenState,
    query: ProductsQuery,
    onQueryChange: (ProductsQuery) -> Unit,
    fetchProducts: (Boolean) -> Unit,
    loadMoreProducts: (ProductsQuery) -> Unit,
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit
) {
    var isRefreshing by rememberSaveable { mutableStateOf(false) }

    LaunchedEffect(Unit) {
        fetchProducts(false)
    }

    LaunchedEffect(state) {
        if (state !is ProductsScreenState.Loading && isRefreshing) {
            isRefreshing = false // stop circular indicator
        }
    }

    Scaffold(
        topBar = {
            ProductsTopBar(
                searchTerm = query.searchTerm.orEmpty(),
                onSearchTermChange = { onQueryChange(query.copy(searchTerm = it)) },
                onSearch = { fetchProducts(true) },
                onBarcodeScanRequest = onBarcodeScanRequest
            )
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                isRefreshing = true
                fetchProducts(true)
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
                    enabled = state is ProductsScreenState.Loaded,
                    query = query,
                    onQueryChange = {
                        onQueryChange(it)
                        fetchProducts(true)
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