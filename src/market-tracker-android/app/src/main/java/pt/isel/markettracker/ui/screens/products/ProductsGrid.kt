package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.screens.products.card.ProductCard

@Composable
fun ProductsGrid(productsState: IOState<List<ProductInfo>>, onProductClick: (String) -> Unit) {
    IOResourceLoader(resource = productsState, errorContent = {
        Text(text = "Error loading products")
    }) { products ->
        LazyVerticalGrid(
            columns = GridCells.Fixed(ProductsScreenViewModel.MAX_GRID_COLUMNS),
            verticalArrangement = Arrangement.spacedBy(10.dp),
            horizontalArrangement = Arrangement.spacedBy(14.dp),
            contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
        ) {
            if (products.isEmpty()) {
                item {
                    Text(text = stringResource(id = R.string.products_not_found))
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
}