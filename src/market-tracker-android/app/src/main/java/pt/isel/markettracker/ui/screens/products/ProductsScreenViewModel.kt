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
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsSortOption
import pt.isel.markettracker.domain.model.market.inventory.product.filter.replaceFacets
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

@HiltViewModel
class ProductsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {
    companion object {
        const val MAX_GRID_COLUMNS = 2
    }

    private val _stateFlow: MutableStateFlow<ProductsScreenState> =
        MutableStateFlow(ProductsScreenState.Idle)
    val stateFlow
        get() = _stateFlow.asStateFlow()

    var query by mutableStateOf(ProductsQuery(sortOption = ProductsSortOption.Relevance))
    private var currentPage by mutableIntStateOf(1)

    fun fetchProducts(forceRefresh: Boolean) {
        val state = _stateFlow.value
        if (state !is ProductsScreenState.Idle && !forceRefresh) return

        currentPage = 1
        _stateFlow.value = ProductsScreenState.Loading

        handleProductsFetch(onFetch = {
            query = query.replaceFacets(it.facets)
        })
    }

    fun loadMoreProducts() {
        val currentState = _stateFlow.value
        if (currentState is ProductsScreenState.LoadingMore ||
            currentState !is ProductsScreenState.Loaded ||
            !currentState.hasMore
        ) return

        _stateFlow.value = ProductsScreenState.LoadingMore(currentState.productsOffers)
        handleProductsFetch()
    }

    private fun handleProductsFetch(
        onFetch: (PaginatedProductOffers) -> Unit = {}
    ) {
        val oldProducts = _stateFlow.value.extractProductsOffers()

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                productService.getProducts(
                    currentPage++,
                    query = query
                )
            }

            res.onSuccess {
                val allProducts = oldProducts + it.items
                _stateFlow.value =
                    ProductsScreenState.IdleLoaded(allProducts, it.hasMore)
                onFetch(it)
            }

            res.onFailure { _stateFlow.value = ProductsScreenState.Failed(it) }
        }
    }
}