package pt.isel.markettracker.ui.screens.products

import android.util.Log
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.lazy.grid.rememberLazyGridState
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.snapshotFlow
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import kotlinx.coroutines.flow.collectLatest
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.ProductsScreenState
import pt.isel.markettracker.ui.screens.extractProducts
import pt.isel.markettracker.ui.screens.products.card.ProductCard

@Composable
fun ProductsGrid(
    state: ProductsScreenState,
    fetchProducts: () -> Unit,
    onProductClick: (String) -> Unit
) {
    val paginationThreshold = 10
    val lazyListState = rememberLazyGridState()

    val products = state.extractProducts()

    // Observe scroll state to load more items
    LaunchedEffect(key1 = lazyListState) {
        snapshotFlow { lazyListState.layoutInfo.visibleItemsInfo.lastOrNull()?.index }
            .collectLatest { itemIndex ->
                if (
                    itemIndex != null
                    && itemIndex >= products.size - paginationThreshold
                ) {
                    Log.v("LeaderboardView", "fetching more items")
                    fetchProducts()
                }
            }
    }

    when (state) {
        is ProductsScreenState.Loaded -> {
            LazyVerticalGrid(
                state = lazyListState,
                columns = GridCells.Fixed(ProductsScreenViewModel.MAX_GRID_COLUMNS),
                verticalArrangement = Arrangement.spacedBy(10.dp),
                horizontalArrangement = Arrangement.spacedBy(14.dp, Alignment.CenterHorizontally),
                contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
            ) {
                if (products.isEmpty()) {
                    item {
                        Text(
                            text = stringResource(id = R.string.products_not_found),
                            color = Color.Red
                        )
                    }
                }
                items(products.size) { index ->
                    Box(
                        contentAlignment = Alignment.Center,
                        modifier = Modifier
                            .size(160.dp, 320.dp)
                    ) {
                        ProductCard(
                            product = products[index],
                            onProductClick = onProductClick
                        )
                    }
                }
            }
        }

        is ProductsScreenState.Error -> {
            Text(
                text = stringResource(id = R.string.products_not_found),
                color = Color.Red
            )
        }

        else -> {
            LoadingIcon()
        }
    }
}