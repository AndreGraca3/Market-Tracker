package pt.isel.markettracker.http.service.operations.product

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.price.CompanyPrices
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.domain.product.ProductReview
import pt.isel.markettracker.domain.product.ProductStats
import pt.isel.markettracker.domain.product.ProductStatsCounts
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.http.service.MarketTrackerService

class ProductService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IProductService, MarketTrackerService() {
    override suspend fun getProducts(querySearch: String?): List<ProductInfo> {
        delay(1000)
        // if query is null, return all products
        val fetchedProducts = if (querySearch == null) dummyProducts
        else dummyProducts.filter { it.name.contains(querySearch, ignoreCase = true) }
        return fetchedProducts.shuffled()
    }

    override suspend fun getProductById(id: String): ProductInfo {
        delay(1000)
        return dummyProducts.first { it.id == id }
    }

    override suspend fun getProductPrices(id: String): List<CompanyPrices> {
        delay(2000)
        return listOf()
    }

    override suspend fun getProductStats(id: String): ProductStats {
        delay(2000)
        return ProductStats(
            id,
            ProductStatsCounts(1, 2, 3),
            3.6,
        )
    }
}