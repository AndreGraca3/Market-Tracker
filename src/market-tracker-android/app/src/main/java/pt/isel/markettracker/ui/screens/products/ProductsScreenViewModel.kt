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
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsFilters
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsSortOption
import pt.isel.markettracker.domain.model.market.inventory.product.filter.replaceFacets
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.ui.screens.products.list.AddToListState
import pt.isel.markettracker.ui.screens.products.list.toSuccess
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

    fun fetchProducts(forceRefresh: Boolean, searchTerm: String? = null) {
        if (_stateFlow.value !is ProductsScreenState.Idle && !forceRefresh) return

        currentPage = 1
        if (query.searchTerm != searchTerm) {
            query = query.copy(searchTerm = searchTerm, filters = ProductsFilters())
        }
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

    private val _addToListStateFlow: MutableStateFlow<AddToListState> =
        MutableStateFlow(AddToListState.Idle)
    val addToListStateFlow
        get() = _addToListStateFlow.asStateFlow()

    fun selectProductToAddToList(productOffer: ProductOffer) {
        if (_addToListStateFlow.value !is AddToListState.Idle) return
        _addToListStateFlow.value = AddToListState.SelectingList(productOffer)
    }

    fun addProductToList(listId: String) {
        val state = _addToListStateFlow.value
        if (state !is AddToListState.SelectingList) return

        _addToListStateFlow.value = AddToListState.AddingToList(
            state.productOffer,
            listId
        )

        viewModelScope.launch {
            val newState = _addToListStateFlow.value
            if (newState !is AddToListState.AddingToList) return@launch

            val res = runCatchingAPIFailure {
                productService.addProductToList(
                    listId,
                    state.productOffer.product.id,
                    state.productOffer.storeOffer.store.id
                )
            }

            res.onSuccess {
                _addToListStateFlow.value = newState.toSuccess()
            }

            res.onFailure { _addToListStateFlow.value = AddToListState.Failed(it) }
        }
    }

    fun resetAddToListState() {
        _addToListStateFlow.value = AddToListState.Idle
    }
}