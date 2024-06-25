package pt.isel.markettracker.ui.screens.products.grid

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.GridItemSpan
import androidx.compose.foundation.lazy.grid.LazyGridState
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.dummy.dummyStoreOffers
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.products.card.ProductCard

@Composable
fun ProductsGrid(
    lazyGridState: LazyGridState,
    hasMore: Boolean,
    productsOffers: List<ProductOffer>,
    onProductClick: (String) -> Unit,
    onAddToListClick: (ProductOffer) -> Unit
) {
    if (productsOffers.isEmpty()) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.verticalScroll(rememberScrollState()) // so user can refresh, useless but useful
        ) {
            Image(
                painter = painterResource(id = R.drawable.products_not_found),
                contentDescription = "No products found"
            )
            Text(
                text = stringResource(id = R.string.products_not_found)
            )
        }
    }

    LazyVerticalGrid(
        state = lazyGridState,
        columns = GridCells.Fixed(ProductsScreenViewModel.MAX_GRID_COLUMNS),
        verticalArrangement = Arrangement.spacedBy(10.dp),
        horizontalArrangement = Arrangement.spacedBy(14.dp, Alignment.CenterHorizontally),
        contentPadding = PaddingValues(12.dp)
    ) {
        items(productsOffers.size, key = { productsOffers[it].product.id }) { index ->
            ProductCard(
                productOffer = productsOffers[index],
                onProductClick = onProductClick,
                onAddToListClick = {
                    onAddToListClick(productsOffers[index])
                },
                modifier = Modifier
                    .width(170.dp)
                    .height(370.dp)
            )
        }
        item(
            span = {
                GridItemSpan(maxCurrentLineSpan)
            }
        ) {
            if (hasMore) {
                Box(
                    contentAlignment = Alignment.Center,
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(30.dp)
                ) {
                    CircularProgressIndicator(
                        modifier = Modifier
                            .fillMaxHeight()
                            .padding(4.dp)
                    )
                }
            }
        }
    }

    val scope = rememberCoroutineScope()
    val showButton by remember {
        derivedStateOf {
            lazyGridState.firstVisibleItemIndex > 0
        }
    }

    ScrollToTopButton(
        visible = showButton,
        onClick = {
            scope.launch {
                lazyGridState.animateScrollToItem(0)
            }
        }
    )
}

@Preview
@Composable
fun ProductsGridPreview() {
    ProductsGrid(
        lazyGridState = LazyGridState(),
        hasMore = true,
        productsOffers = dummyProducts.map { ProductOffer(it, dummyStoreOffers.first()) },
        onProductClick = {},
        onAddToListClick = {}
    )
}