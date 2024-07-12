package pt.isel.markettracker.ui

import io.mockk.coEvery
import io.mockk.mockk
import kotlinx.coroutines.test.StandardTestDispatcher
import org.junit.Rule
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.ProductsFacetsCounters
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.utils.MockMainDispatcherRule

class ProductsScreenViewModelTests {
    @get:Rule
    val rule = MockMainDispatcherRule(testDispatcher = StandardTestDispatcher())

    companion object {
        private const val NON_EXISTING_PAGE = -1
        private const val NON_EXISTING_ID = -1
        private const val FIRST_PAGE = 1
        private const val SECOND_PAGE = 2
    }

    private val mockProductService = mockk<IProductService> {
        coEvery {
            getProducts(
                page = any(),
                query = any()
            )
        } returns PaginatedProductOffers(
            items = emptyList(),
            facets = ProductsFacetsCounters(emptyList(), emptyList(), emptyList()),
            currentPage = FIRST_PAGE,
            itemsPerPage = 0,
            totalItems = 0,
            totalPages = 0
        )
    }
}