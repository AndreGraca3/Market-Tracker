package pt.isel.markettracker.http.service.operations.product

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import okhttp3.Request
import pt.isel.markettracker.domain.model.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.product.ProductInfo
import pt.isel.markettracker.domain.model.product.ProductPreferences
import pt.isel.markettracker.domain.model.product.ProductStats
import pt.isel.markettracker.domain.model.product.ProductStatsCounts
import pt.isel.markettracker.dummy.dummyCompanyPrices
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.http.models.price.CompanyPrices
import pt.isel.markettracker.http.service.MarketTrackerService
import java.net.URL

private fun buildProductsUrl(
    page: Int,
    itemsPerPage: Int?,
    searchQuery: String?,
    sortOption: String?
) = "/products?page=$page" +
        itemsPerPage?.let { "&itemsPerPage=$it" }.orEmpty() +
        searchQuery?.let { "&name=$it" }.orEmpty() +
        sortOption?.let { "&sortBy=$it" }.orEmpty()

private const val PRODUCT_URL = "/products/{id}"

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
        return requestHandler<PaginatedProductOffers>(
            request = Request.Builder().buildRequest(
                url = URL(
                    buildProductsUrl(
                        page = page,
                        itemsPerPage = itemsPerPage,
                        searchQuery = searchQuery,
                        sortOption = sortOption
                    )
                ),
                method = HttpMethod.GET
            )
        )
    }

    override suspend fun getProductById(id: String): ProductInfo {
        delay(1000)
        return dummyProducts.first { it.id == id }
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
            null,
            null,
        )
    }
}