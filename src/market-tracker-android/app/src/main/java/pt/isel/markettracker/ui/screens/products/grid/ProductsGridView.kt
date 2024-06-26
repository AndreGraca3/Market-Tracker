package pt.isel.markettracker.ui.screens.products.grid

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.grid.rememberLazyGridState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.products.ProductsScreenState
import pt.isel.markettracker.ui.screens.products.extractHasMore
import pt.isel.markettracker.ui.screens.products.extractProductsOffers

@Composable
fun ProductsGridView(
    state: ProductsScreenState,
    loadMoreProducts: () -> Unit,
    onProductClick: (String) -> Unit,
    onAddToListClick: (ProductOffer) -> Unit
) {
    val productsOffers = state.extractProductsOffers()

    val scrollState = rememberLazyGridState()
    val isItemReachEndScroll by remember {
        derivedStateOf {
            scrollState.layoutInfo.visibleItemsInfo.lastOrNull()?.index ==
                    scrollState.layoutInfo.totalItemsCount - 1
        }
    }

    LaunchedEffect(key1 = isItemReachEndScroll) {
        if (isItemReachEndScroll && state.extractHasMore()) {
            loadMoreProducts()
        }
    }

    LaunchedEffect(state) {
        if (state is ProductsScreenState.Loading) {
            scrollState.scrollToItem(0)
        }
    }

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier.fillMaxSize()
    ) {
        when (state) {
            is ProductsScreenState.Loaded -> {
                ProductsGrid(
                    lazyGridState = scrollState,
                    hasMore = state.hasMore,
                    productsOffers = productsOffers,
                    onProductClick = onProductClick,
                    onAddToListClick = { productOffer ->
                        onAddToListClick(productOffer)
                    }
                )
            }

            is ProductsScreenState.Failed -> {
                LazyColumn( // TODO: remove maybe?
                    modifier = Modifier.fillMaxSize(),
                    verticalArrangement = Arrangement.Center,
                    horizontalAlignment = Alignment.CenterHorizontally
                ) {
                    item {
                        Image(
                            painter = painterResource(id = R.drawable.server_error),
                            contentDescription = "No products"
                        )
                    }
                }
            }

            else -> {
                LoadingIcon(stringResource(id = R.string.products_loading))
            }
        }
    }
}