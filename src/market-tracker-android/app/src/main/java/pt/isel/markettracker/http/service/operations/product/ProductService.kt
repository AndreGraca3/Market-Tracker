package pt.isel.markettracker.http.service.operations.product

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStatsCounts
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.dummy.dummyCompanyPrices
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.http.service.MarketTrackerService
import java.time.LocalDateTime

private fun buildProductsPath(
    page: Int,
    itemsPerPage: Int?,
    searchQuery: String?,
    sortOption: String?
) = "/products?page=$page" +
        itemsPerPage?.let { "&itemsPerPage=$it" }.orEmpty() +
        searchQuery?.let { "&name=$it" }.orEmpty() //+
        //sortOption?.let { "&sortBy=$it" }.orEmpty()

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
        return requestHandler(
            path = buildProductsPath(
                page = page,
                itemsPerPage = itemsPerPage,
                searchQuery = searchQuery,
                sortOption = sortOption
            ),
            method = HttpMethod.GET
        )
    }

    override suspend fun getProductById(productId: String): Product {
        /*return requestHandler(
            path = buildProductByIdPath(id),
            method = HttpMethod.GET
        )*/
        return dummyProducts.first()
    }

    override suspend fun getProductPrices(productId: String): List<CompanyPrices> {
        delay(1500)
        return dummyCompanyPrices
    }

    override suspend fun getProductStats(productId: String): ProductStats {
        delay(1500)
        return ProductStats(
            productId,
            ProductStatsCounts(1, 2, 3),
            3.6,
        )
    }

    override suspend fun getProductPreferences(productId: String): ProductPreferences {
        delay(1500)
        return ProductPreferences(
            false,
            ProductReview(
                "1",
                3,
                "Gostei bastante do produto, recomendo e penso comprar mais vezes mas o preço é um pouco alto.",
                ClientItem(
                    "1",
                    "João",
                    "https://i.imgur.com/fL67hTu.jpeg"
                ),
                LocalDateTime.now()
            )
        )
    }

    override suspend fun getProductAlerts(productId: String): List<PriceAlert> {
        delay(1500)
        return List(10) {
            PriceAlert(
                "1",
                3,
                35
            )
        }
    }

    override suspend fun getProductReviews(
        productId: String,
        page: Int,
        itemsPerPage: Int?
    ): List<ProductReview> {
        delay(5000)

        val review = ProductReview(
            "1",
            3,
            "Gostei bastante do produto, recomendo e penso comprar mais vezes mas o preço é um pouco alto.",
            ClientItem(
                "1",
                "João",
                "https://i.imgur.com/fL67hTu.jpeg"
            ),
            LocalDateTime.now()
        )

        return List(10) { review }
    }

    override suspend fun submitProductReview(
        productId: String,
        rating: Int,
        review: String?
    ): ProductReview {
        delay(1500)
        return ProductReview(
            "1",
            rating,
            review,
            ClientItem(
                "1",
                "João",
                null
            ),
            LocalDateTime.now()
        )
    }
}