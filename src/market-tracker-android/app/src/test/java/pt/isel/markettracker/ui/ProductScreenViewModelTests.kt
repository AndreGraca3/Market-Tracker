package pt.isel.markettracker.ui

import io.mockk.coEvery
import io.mockk.coVerify
import io.mockk.mockk
import junit.framework.TestCase.assertEquals
import kotlinx.coroutines.cancel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.test.StandardTestDispatcher
import kotlinx.coroutines.test.runTest
import org.junit.Rule
import org.junit.Test
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStatsCounts
import pt.isel.markettracker.domain.model.market.price.PriceAlertId
import pt.isel.markettracker.domain.model.market.price.ProductPrices
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.http.service.operations.alert.IAlertService
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.repository.auth.AuthState
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.ui.screens.product.ProductDetailsScreenState
import pt.isel.markettracker.ui.screens.product.ProductDetailsScreenViewModel
import pt.isel.markettracker.ui.screens.product.extractProduct
import pt.isel.markettracker.ui.screens.product.rating.ProductPreferencesState
import pt.isel.markettracker.utils.MockMainDispatcherRule
import pt.isel.markettracker.utils.flows.subscribeBeforeCallingOperation
import java.time.LocalDateTime

class ProductScreenViewModelTests {
    @get:Rule
    val rule = MockMainDispatcherRule(testDispatcher = StandardTestDispatcher())

    private val startTestsTime = LocalDateTime.now()

    private val mockProductService = mockk<IProductService> {
        coEvery {
            getProductById(any())
        } returns dummyProducts.first()

        coEvery {
            getProductStats(any())
        } returns ProductStats("1", ProductStatsCounts(0, 0, 0), 1.0)

        coEvery {
            getProductPrices(any())
        } returns ProductPrices(emptyList(), 0, 0)

        coEvery {
            getProductPreferences(any())
        } returns ProductPreferences(
            true,
            ProductReview(1, "1", 1, null, startTestsTime, ClientItem("id1", "username", null))
        )

        coEvery {
            submitProductReview(any(), any(), any())
        } returns ProductReview(
            1,
            "1",
            1,
            null,
            startTestsTime,
            ClientItem("id1", "username", null)
        )
    }

    private val mockAlertService = mockk<IAlertService> {
        coEvery {
            createAlert(any(), any(), any())
        } returns PriceAlertId("1")
    }

    private val mockAuthRepository = mockk<IAuthRepository> {
        coEvery {
            authState
        } returns MutableStateFlow(AuthState.Loaded(emptyList(), emptyList()))
    }

    @Test
    fun `fetchProduct should fetch product when state is Idle`() = runTest {
        // Arrange
        val viewModel =
            ProductDetailsScreenViewModel(mockProductService, mockAlertService, mockAuthRepository)
        val product = dummyProducts.first()

        // Act
        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.fetchProductById(product.id)
        }.also { collectedStates ->
            // Assert
            assertEquals(product, collectedStates.last().extractProduct())
        }
    }

    @Test
    fun `fetchProduct should not fetch product when state is not Idle`() = runTest {
        // Arrange
        val viewModel =
            ProductDetailsScreenViewModel(mockProductService, mockAlertService, mockAuthRepository)
        val product = dummyProducts.first()

        // Act
        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.fetchProductById(product.id)
            viewModel.fetchProductById(product.id)
        }.also { collectedStates ->
            // Assert
            assertEquals(product, collectedStates.last().extractProduct())
        }

        coVerify(exactly = 1) {
            mockProductService.getProductById(product.id)
        }
    }

    @Test
    fun `fetchProductDetails should not fetch details if state is not LoadedProduct`() = runTest {
        // Arrange
        val viewModel =
            ProductDetailsScreenViewModel(mockProductService, mockAlertService, mockAuthRepository)
        val product = dummyProducts.first()

        // Act
        viewModel.stateFlow.subscribeBeforeCallingOperation {
            viewModel.fetchProductById(product.id)
            viewModel.fetchProductDetails(product.id)
        }.also { collectedStates ->
            // Assert
            assertEquals(product, collectedStates.last().extractProduct())
        }

        coVerify(exactly = 1) {
            mockProductService.getProductById(product.id)
        }
    }

    @Test
    fun `fetchProductDetails should fetch details if state is LoadedProduct`() = runTest {
        // Arrange
        val viewModel =
            ProductDetailsScreenViewModel(mockProductService, mockAlertService, mockAuthRepository)
        val product = dummyProducts.first()

        // Act
        viewModel.fetchProductById(product.id)

        // Assert
        this.launch {
            viewModel.stateFlow.collect { state ->
                if (state is ProductDetailsScreenState.LoadedProduct) {
                    viewModel.fetchProductDetails(product.id)
                    assertEquals(
                        viewModel.stateFlow.value,
                        ProductDetailsScreenState.LoadingProductDetails(product)
                    )
                }

                if (state is ProductDetailsScreenState.LoadedDetails) {
                    coVerify(exactly = 1) {
                        mockProductService.getProductStats(product.id)
                        mockProductService.getProductPrices(product.id)
                        mockProductService.getProductPreferences(product.id)
                    }
                    this.cancel()
                }
            }
        }
    }

    @Test
    fun `submitReview should submit review when PreferencesState is Loaded`() = runTest {
        // Arrange
        val viewModel =
            ProductDetailsScreenViewModel(mockProductService, mockAlertService, mockAuthRepository)
        val product = dummyProducts.first()
        viewModel.fetchProductById(product.id)

        var firstTime = true

        this.launch {
            viewModel.stateFlow.collect {
                if (it is ProductDetailsScreenState.LoadedProduct) {
                    viewModel.fetchProductDetails(product.id)
                }

                if (it is ProductDetailsScreenState.LoadedDetails) {
                    if (!firstTime) return@collect
                    firstTime = false

                    // Act
                    viewModel.prefsStateFlow.subscribeBeforeCallingOperation {
                        viewModel.submitUserRating(product.id, 5, "Great product")
                    }.also { collectedStates ->
                        // Assert
                        assertEquals(
                            ProductPreferencesState.Loaded(
                                ProductPreferences(
                                    true,
                                    ProductReview(
                                        1,
                                        "1",
                                        1,
                                        null,
                                        startTestsTime,
                                        ClientItem("id1", "username", null)
                                    )
                                )
                            ),
                            collectedStates.last()
                        )
                        this.cancel()
                    }
                }
            }
        }
    }
}