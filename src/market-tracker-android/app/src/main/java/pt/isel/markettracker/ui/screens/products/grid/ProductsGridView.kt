package pt.isel.markettracker.ui.screens.products.grid

import android.util.Log
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.grid.rememberLazyGridState
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import com.example.markettracker.R
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.ProductsScreenState
import pt.isel.markettracker.ui.screens.extractProductsOffers

@Composable
fun ProductsGridView(
    state: ProductsScreenState,
    loadMoreProducts: () -> Unit,
    onProductClick: (String) -> Unit
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
        if (isItemReachEndScroll) {
            Log.v("Products", "fetching more items")
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
                    productsOffers = productsOffers,
                    onProductClick = onProductClick
                )
            }

            is ProductsScreenState.Error -> {
                Text(
                    text = stringResource(id = R.string.product_loading_error),
                    color = Color.Red
                )
            }

            else -> {
                LoadingIcon(stringResource(id = R.string.products_loading))
            }
        }
    }
}