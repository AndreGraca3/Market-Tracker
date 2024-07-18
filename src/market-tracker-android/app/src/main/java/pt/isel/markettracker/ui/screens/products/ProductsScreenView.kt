package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
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
import androidx.compose.ui.unit.dp
import com.talhafaki.composablesweettoast.util.SweetToastUtil
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.products.filters.FilterOptionsRow
import pt.isel.markettracker.ui.screens.products.grid.ProductsGridView
import pt.isel.markettracker.ui.screens.products.list.AddToListState
import pt.isel.markettracker.ui.screens.products.list.ListsBottomSheet
import pt.isel.markettracker.ui.screens.products.topbar.ProductsTopBar

@Composable
fun ProductsScreenView(
    state: ProductsScreenState,
    query: ProductsQuery,
    onQueryChange: (ProductsQuery) -> Unit,
    fetchProducts: (String?) -> Unit,
    loadMoreProducts: (ProductsQuery) -> Unit,
    onProductClick: (String) -> Unit,
    shoppingLists: List<ShoppingList>,
    addToListState: AddToListState,
    onAddToListClick: (ProductOffer) -> Unit,
    onListSelectedClick: (String) -> Unit,
    onAddToListDismissRequest: () -> Unit,
    onBarcodeScanRequest: () -> Unit,
) {
    var isRefreshing by rememberSaveable { mutableStateOf(false) }

    LaunchedEffect(state) {
        if (state !is ProductsScreenState.Loading && isRefreshing) {
            isRefreshing = false // stop circular indicator
        }
    }

    Scaffold(
        topBar = {
            ProductsTopBar(
                searchTerm = query.searchTerm.orEmpty(),
                onSearch = fetchProducts,
                onBarcodeScanRequest = onBarcodeScanRequest
            )
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                isRefreshing = true
                fetchProducts(query.searchTerm)
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
                        fetchProducts(it.searchTerm)
                    },
                    isLoading = state is ProductsScreenState.Loading
                )

                ProductsGridView(
                    state = state,
                    loadMoreProducts = { loadMoreProducts(query) },
                    onProductClick = onProductClick,
                    onAddToListClick = onAddToListClick
                )

                when (addToListState) {
                    is AddToListState.SelectingList ->
                        ListsBottomSheet(
                            shoppingLists = shoppingLists,
                            onListSelectedClick = onListSelectedClick,
                            onDismissRequest = onAddToListDismissRequest
                        )

                    is AddToListState.Success -> {
                        SweetToastUtil.SweetSuccess(
                            message = "Product added to list",
                            padding = PaddingValues(42.dp),
                            contentAlignment = Alignment.BottomCenter
                        )
                    }

                    is AddToListState.Failed -> {
                        SweetToastUtil.SweetError(
                            message = addToListState.error.localizedMessage
                                ?: "Failed to add product to list", padding = PaddingValues(42.dp),
                            contentAlignment = Alignment.BottomCenter
                        )
                    }

                    else -> { /* do nothing */
                    }
                }
            }
        }
    }
}