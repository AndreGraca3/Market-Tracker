package pt.isel.markettracker.ui.screens.products

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import pt.isel.markettracker.repository.auth.IAuthRepository

@Composable
fun ProductsScreen(
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit,
    navigateToLogin: () -> Unit,
    authRepository: IAuthRepository,
    productsScreenViewModel: ProductsScreenViewModel
) {
    val screenState by productsScreenViewModel.stateFlow.collectAsState()
    val addToListState by productsScreenViewModel.addToListStateFlow.collectAsState()

    ProductsScreenView(
        state = screenState,
        query = productsScreenViewModel.query,
        onQueryChange = { productsScreenViewModel.query = it },
        fetchProducts = { forceRefresh ->
            productsScreenViewModel.fetchProducts(forceRefresh)
        },
        loadMoreProducts = { productsScreenViewModel.loadMoreProducts() },
        onProductClick = onProductClick,
        shoppingLists = authRepository.getLists(),
        addToListState = addToListState,
        onAddToListClick = { productOffer ->
            if (authRepository.isUserLoggedIn())
                productsScreenViewModel.selectProductToAddToList(productOffer)
            else navigateToLogin()
        },
        onListSelectedClick = { listId ->
            productsScreenViewModel.addProductToList(listId)
        },
        onAddToListDismissRequest = { productsScreenViewModel.resetAddToListState() },
        onBarcodeScanRequest = onBarcodeScanRequest
    )
}