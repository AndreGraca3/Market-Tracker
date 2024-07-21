package pt.isel.markettracker.ui.screens.products

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.repository.auth.extractLists
import pt.isel.markettracker.repository.auth.isLoggedIn

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
    val authState by authRepository.authState.collectAsState()

    ProductsScreenView(
        state = screenState,
        query = productsScreenViewModel.query,
        onQueryChange = { productsScreenViewModel.query = it },
        fetchProducts = { productsScreenViewModel.fetchProducts(true, it) },
        loadMoreProducts = { productsScreenViewModel.loadMoreProducts() },
        onProductClick = onProductClick,
        shoppingLists = authState.extractLists().filter { !it.isArchived },
        addToListState = addToListState,
        onAddToListClick = { productOffer ->
            if (authState.isLoggedIn())
                productsScreenViewModel.selectProductToAddToList(productOffer)
            else navigateToLogin()
        },
        onListSelectedClick = productsScreenViewModel::addProductToList,
        onAddToListDismissRequest = { productsScreenViewModel.resetAddToListState() },
        onBarcodeScanRequest = onBarcodeScanRequest
    )
}