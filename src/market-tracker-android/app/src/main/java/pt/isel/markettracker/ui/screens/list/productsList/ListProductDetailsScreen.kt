package pt.isel.markettracker.ui.screens.list.productsList

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
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
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ListProductDetailsScreen(
    listProductDetailsScreenViewModel: ListProductDetailsScreenViewModel = hiltViewModel(),
    onBackRequest: () -> Unit
) {
    LaunchedEffect(Unit) {
        listProductDetailsScreenViewModel.fetchProducts()
    }
    val scope = rememberCoroutineScope()
    var isRefreshing by remember { mutableStateOf(false) }

    val listEntriesState by listProductDetailsScreenViewModel.listProduct.collectAsState()

    Scaffold(
        topBar = {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Red)
                    .padding(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                HeaderLogo()
                Text(
                    "A lista de compras", // TODO: passar um intent com o nome da lista...
                    color = Color.White,
                    fontFamily = mainFont,
                    fontSize = 30.sp
                )
            }
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                scope.launch {
                    isRefreshing = true
                    listProductDetailsScreenViewModel.fetchProducts(true)
                    listProductDetailsScreenViewModel.listProduct.collect {
                        if (it !is Loading) {
                            isRefreshing = false
                        }
                    }
                }
            },
            modifier = Modifier
                .padding(paddingValues)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize(),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                ProductList(listEntriesState)
            }
        }
    }
}