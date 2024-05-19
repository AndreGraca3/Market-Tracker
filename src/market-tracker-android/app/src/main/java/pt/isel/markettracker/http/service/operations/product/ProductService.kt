package pt.isel.markettracker.http.service.operations.product

import android.util.Log
import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.market.inventory.product.FacetCounter
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStatsCounts
import pt.isel.markettracker.domain.model.market.inventory.product.ProductsFacetsCounters
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.dummy.dummyBrands
import pt.isel.markettracker.dummy.dummyCategories
import pt.isel.markettracker.dummy.dummyCompanies
import pt.isel.markettracker.dummy.dummyCompanyPrices
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.http.service.MarketTrackerService

private fun buildProductsPath(
    page: Int,
    itemsPerPage: Int?,
    searchQuery: String?,
    sortOption: String?
) = "/products?page=$page" +
        itemsPerPage?.let { "&itemsPerPage=$it" }.orEmpty() +
        searchQuery?.let { "&name=$it" }.orEmpty() +
        sortOption?.let { "&sortBy=$it" }.orEmpty()

private fun buildProductByIdPath(id: String) = "/products/$id"

class ProductService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IProductService, MarketTrackerService() {
    override suspend fun getProducts(
        page: Int,
        itemsPerPage: Int?,
        searchQuery: String?,
        sortOption: String?
    ): PaginatedProductOffers {
        /*return requestHandler(
            path = buildProductsPath(
                page = page,
                itemsPerPage = itemsPerPage,
                searchQuery = searchQuery,
                sortOption = sortOption
            ),
            method = HttpMethod.GET
        )*/
        Log.v("ProductService", "Fetching products")
        delay(3000)
        return PaginatedProductOffers(
            dummyProducts.shuffled().map {
                ProductOffer(
                    it,
                    dummyCompanyPrices.first().storePrices.first()
                )
            },
            ProductsFacetsCounters(
                listOf(
                    FacetCounter(
                        dummyBrands.random(),
                        (100..230).random()
                    ),
                    FacetCounter(
                        dummyBrands.random(),
                        (200..300).random()
                    )
                ),
                listOf(
                    FacetCounter(
                        dummyCompanies.random(),
                        (100..230).random()
                    ),
                    FacetCounter(
                        dummyCompanies.random(),
                        (100..230).random()
                    )
                ),
                listOf(
                    FacetCounter(
                        dummyCategories.random(),
                        (1..10).random()
                    ),
                    FacetCounter(
                        dummyCategories.random(),
                        (1..10).random()
                    )
                )
            ),
            0,
            0,
            0,
            0,
            true
        )
    }

    override suspend fun getProductById(id: String): Product {
        /*return requestHandler(
            path = buildProductByIdPath(id),
            method = HttpMethod.GET
        )*/
        return dummyProducts.first()
    }

    override suspend fun getProductPrices(id: String): List<CompanyPrices> {
        delay(1500)
        return dummyCompanyPrices
    }

    override suspend fun getProductStats(id: String): ProductStats {
        delay(1500)
        return ProductStats(
            id,
            ProductStatsCounts(1, 2, 3),
            3.6,
        )
    }

    override suspend fun getProductPreferences(id: String): ProductPreferences {
        delay(1500)
        return ProductPreferences(
            false,
            null
        )
    }

    override suspend fun getProductAlerts(id: String): List<PriceAlert> {
        delay(1500)
        return listOf(
            PriceAlert(
                "1",
                10,
                100
            )
        )
    }
}