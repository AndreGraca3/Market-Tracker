package pt.isel.markettracker.ui.screens.products

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.inventory.product.filter.replaceWithState
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.ui.screens.ProductsScreenState
import pt.isel.markettracker.ui.screens.extractProductsOffers
import javax.inject.Inject

@HiltViewModel
class ProductsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {
    companion object {
        const val MAX_GRID_COLUMNS = 2
    }

    // actual listed products
    private val _stateFlow: MutableStateFlow<ProductsScreenState> =
        MutableStateFlow(ProductsScreenState.Idle)
    val stateFlow
        get() = _stateFlow.asStateFlow()

    var query by mutableStateOf(ProductsQuery())

    private var currentPage by mutableIntStateOf(1)

    fun fetchProducts(productsQuery: ProductsQuery, forceRefresh: Boolean) {
        if (_stateFlow.value !is ProductsScreenState.Idle && !forceRefresh) return

        currentPage = 1
        _stateFlow.value = ProductsScreenState.Loading
        handleProductsFetch(productsQuery)
    }

    fun loadMoreProducts(productsQuery: ProductsQuery) {
        val currentState = _stateFlow.value
        if (currentState is ProductsScreenState.LoadingMore ||
            currentState !is ProductsScreenState.Loaded ||
            !currentState.hasMore
        ) return

        _stateFlow.value = ProductsScreenState.LoadingMore(currentState.productsOffers)
        handleProductsFetch(productsQuery)
    }

    private fun handleProductsFetch(productsQuery: ProductsQuery) {
        val oldProducts = _stateFlow.value.extractProductsOffers()

        viewModelScope.launch {
            val res = kotlin.runCatching {
                productService.getProducts(
                    currentPage++,
                    searchQuery = productsQuery.searchTerm,
                    sortOption = productsQuery.sortOption.name
                )
            }

            res.onSuccess {
                val allProducts = oldProducts + it.items
                _stateFlow.value = ProductsScreenState.IdleLoaded(allProducts, it.hasMore)
                query =
                    productsQuery.copy(filters = productsQuery.filters.replaceWithState(it.facets))
            }
            res.onFailure { _stateFlow.value = ProductsScreenState.Error(it) }
        }
    }
}