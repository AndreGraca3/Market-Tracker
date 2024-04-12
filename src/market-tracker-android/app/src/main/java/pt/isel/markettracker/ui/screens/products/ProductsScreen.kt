package pt.isel.markettracker.ui.screens.products

import android.annotation.SuppressLint
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
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
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.products.card.ProductCard
import pt.isel.markettracker.ui.screens.products.topbar.ProductsTopBar

@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun ProductsScreen(
    onProductClick: (String) -> Unit,
    productsScreenViewModel: ProductsScreenViewModel = hiltViewModel(),
) {
    LaunchedEffect(Unit) {
        productsScreenViewModel.fetchProducts()
    }

    var isRefreshing by remember { mutableStateOf(false) }

    val productsState by productsScreenViewModel.products.collectAsState()

    val scope = rememberCoroutineScope()

    Scaffold(
        topBar = {
            ProductsTopBar()
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
            IOResourceLoader(resource = productsState, errorContent = {
                Text(text = "Error loading products")
            }) { products ->
                LazyVerticalGrid(
                    columns = GridCells.Fixed(ProductsScreenViewModel.MAX_GRID_COLUMNS),
                    verticalArrangement = Arrangement.spacedBy(10.dp),
                    horizontalArrangement = Arrangement.spacedBy(14.dp),
                    contentPadding =  PaddingValues(horizontal = 16.dp, vertical = 10.dp),
                ) {
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
    }
}