package pt.isel.markettracker.ui

import io.mockk.coEvery
import io.mockk.coVerify
import io.mockk.mockk
import junit.framework.TestCase.assertEquals
import kotlinx.coroutines.test.StandardTestDispatcher
import kotlinx.coroutines.test.runTest
import org.junit.Rule
import org.junit.Test
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.domain.model.market.inventory.product.ProductsFacetsCounters
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.dummy.dummyStoreOffers
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.ui.screens.products.ProductsScreenState
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.products.extractProductsOffers
import pt.isel.markettracker.ui.screens.products.list.AddToListState
import pt.isel.markettracker.utils.MockMainDispatcherRule
import pt.isel.markettracker.utils.flows.subscribeBeforeCallingOperation

class ProductsScreenViewModelTests {
    @get:Rule
    val rule = MockMainDispatcherRule(testDispatcher = StandardTestDispatcher())

    companion object {
        private const val PAGE = 1
    }

    private val mockProductService = mockk<IProductService> {
        coEvery {
            getProducts(page = any(), itemsPerPage = any(), query = any())
        } returns PaginatedProductOffers(
            items = dummyStoreOffers.mapIndexed { index, storeOffer ->
                ProductOffer(
                    dummyProducts[index], storeOffer
                )
            },
            facets = ProductsFacetsCounters(emptyList(), emptyList(), emptyList()),
            currentPage = PAGE,
            itemsPerPage = 0,
            totalItems = dummyStoreOffers.size,
            totalPages = 2
        )

        coEvery {
            addProductToList(any(), any(), any())
        } returns Unit
    }

    @Test
    fun `getProducts without forceRefresh should fetch products when screen is Idle`() = runTest {
        val viewModel = ProductsScreenViewModel(mockProductService)

        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.fetchProducts(false)
        }.also { collectedStates ->
            val expectedStates = listOf(
                ProductsScreenState.Idle,
                ProductsScreenState.Loading,
                ProductsScreenState.IdleLoaded(
                    productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    },
                    hasMore = true
                )
            )
            assertEquals(expectedStates, collectedStates)
        }

        assertEquals(dummyStoreOffers.size, viewModel.stateFlow.value.extractProductsOffers().size)

        coVerify(exactly = 1) {
            mockProductService.getProducts(
                page = PAGE,
                itemsPerPage = any(),
                query = any()
            )
        }
    }

    @Test
    fun `getProducts without forceRefresh should not fetch products when screen is not Idle`() =
        runTest {
            val viewModel = ProductsScreenViewModel(mockProductService)

            viewModel.stateFlow.subscribeBeforeCallingOperation {
                viewModel.fetchProducts(false)
                viewModel.fetchProducts(false)
            }.also { collectedStates ->
                val expectedStates = listOf(
                    ProductsScreenState.Idle,
                    ProductsScreenState.Loading,
                    ProductsScreenState.IdleLoaded(
                        productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                            ProductOffer(dummyProducts[index], storeOffer)
                        },
                        hasMore = true
                    )
                )
                assertEquals(expectedStates, collectedStates)
            }

            assertEquals(
                dummyStoreOffers.size,
                viewModel.stateFlow.value.extractProductsOffers().size
            )

            coVerify(exactly = 1) {
                mockProductService.getProducts(
                    page = PAGE,
                    itemsPerPage = any(),
                    query = any()
                )
            }
        }

    @Test
    fun `getProducts with forceRefresh should fetch products when screen is not Idle`() = runTest {
        val viewModel = ProductsScreenViewModel(mockProductService)

        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.fetchProducts(false)
        }.also { collectedStates ->
            val expectedStates = listOf(
                ProductsScreenState.Idle,
                ProductsScreenState.Loading,
                ProductsScreenState.IdleLoaded(
                    productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    },
                    hasMore = true
                )
            )
            assertEquals(expectedStates, collectedStates)
        }

        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.fetchProducts(true)
        }.also { collectedStates ->
            val expectedStates = listOf(
                ProductsScreenState.IdleLoaded(
                    productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    },
                    hasMore = true
                ),
                ProductsScreenState.Loading,
                ProductsScreenState.IdleLoaded(
                    productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    },
                    hasMore = true
                )
            )
            assertEquals(expectedStates, collectedStates)
        }

        assertEquals(dummyStoreOffers.size, viewModel.stateFlow.value.extractProductsOffers().size)

        coVerify(exactly = 2) {
            mockProductService.getProducts(
                page = PAGE,
                itemsPerPage = any(),
                query = any()
            )
        }
    }

    @Test
    fun `loadMoreProducts should fetch more products when screen is Loaded`() = runTest {
        val viewModel = ProductsScreenViewModel(mockProductService)

        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.fetchProducts(false)
        }.also { collectedStates ->
            val expectedStates = listOf(
                ProductsScreenState.Idle,
                ProductsScreenState.Loading,
                ProductsScreenState.IdleLoaded(
                    productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    },
                    hasMore = true
                )
            )
            assertEquals(expectedStates, collectedStates)
        }

        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.loadMoreProducts()
        }.also { collectedStates ->
            val initialState = ProductsScreenState.IdleLoaded(
                productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                    ProductOffer(dummyProducts[index], storeOffer)
                },
                hasMore = true
            )

            val expectedStates = listOf(
                initialState,
                ProductsScreenState.LoadingMore(
                    productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    }
                ),
                initialState.copy(
                    productsOffers = dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    } + dummyStoreOffers.mapIndexed { index, storeOffer ->
                        ProductOffer(dummyProducts[index], storeOffer)
                    }
                )
            )
            assertEquals(expectedStates, collectedStates)
        }

        assertEquals(
            dummyStoreOffers.size * 2,
            viewModel.stateFlow.value.extractProductsOffers().size
        )

        coVerify(exactly = 2) {
            mockProductService.getProducts(
                page = any(),
                itemsPerPage = any(),
                query = any()
            )
        }
    }

    @Test
    fun `add product to list should change state to SelectingList`() = runTest {
        val viewModel = ProductsScreenViewModel(mockProductService)

        viewModel.addToListStateFlow.subscribeBeforeCallingOperation {
            viewModel.selectProductToAddToList(
                ProductOffer(dummyProducts.first(), dummyStoreOffers.first())
            )
        }.also { collectedStates ->
            val expectedStates = listOf(
                AddToListState.Idle,
                AddToListState.SelectingList(
                    ProductOffer(
                        dummyProducts.first(),
                        dummyStoreOffers.first()
                    )
                )
            )
            assertEquals(expectedStates, collectedStates)
        }
    }

    @Test
    fun `add product to list should change state to AddingToList`() = runTest {
        val viewModel = ProductsScreenViewModel(mockProductService)

        viewModel.addToListStateFlow.subscribeBeforeCallingOperation {
            viewModel.selectProductToAddToList(
                ProductOffer(dummyProducts.first(), dummyStoreOffers.first())
            )
            viewModel.addProductToList("1")
        }.also { collectedStates ->
            val expectedStates = listOf(
                AddToListState.Idle,
                AddToListState.AddingToList(
                    ProductOffer(
                        dummyProducts.first(),
                        dummyStoreOffers.first()
                    ),
                    "1"
                ),
                AddToListState.Success(
                    ProductOffer(
                        dummyProducts.first(),
                        dummyStoreOffers.first()
                    ),
                    "1"
                )
            )
            assertEquals(expectedStates, collectedStates)
        }
    }

    @Test
    fun `add product to list should not change state to AddingToList when state is not SelectingList`() = runTest {
        val viewModel = ProductsScreenViewModel(mockProductService)

        viewModel.addToListStateFlow.subscribeBeforeCallingOperation {
            viewModel.addProductToList("1")
        }.also { collectedStates ->
            val expectedStates = listOf(
                AddToListState.Idle
            )
            assertEquals(expectedStates, collectedStates)
        }
    }
}