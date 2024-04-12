package pt.isel.markettracker.http.service.operations.product

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.http.service.MarketTrackerService

class ProductService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IProductService, MarketTrackerService() {
    override suspend fun getProducts(): List<ProductInfo> {
        delay(1000)
        return dummyProducts.shuffled()
    }
}