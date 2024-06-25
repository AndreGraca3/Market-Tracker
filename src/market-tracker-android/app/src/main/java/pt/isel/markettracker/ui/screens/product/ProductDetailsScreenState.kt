package pt.isel.markettracker.ui.screens.product

import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.price.ProductPrices

sealed class ProductDetailsScreenState {
    data object Idle : ProductDetailsScreenState()
    data object LoadingProduct : ProductDetailsScreenState()
    data class LoadedProduct(val product: Product) : ProductDetailsScreenState()
    data class LoadingProductDetails(
        val product: Product,
        val prices: ProductPrices? = null,
        val stats: ProductStats? = null,
    ) : ProductDetailsScreenState()

    abstract class Loaded(
        open val product: Product,
        open val prices: ProductPrices,
        open val stats: ProductStats,
        open val paginatedReviews: PaginatedResult<ProductReview>?
    ) : ProductDetailsScreenState()

    data class LoadedDetails(
        override val product: Product,
        override val prices: ProductPrices,
        override val stats: ProductStats,
        override val paginatedReviews: PaginatedResult<ProductReview>?
    ) : Loaded(product, prices, stats, paginatedReviews)

    data class LoadingReviews(
        override val product: Product,
        override val prices: ProductPrices,
        override val stats: ProductStats,
        override val paginatedReviews: PaginatedResult<ProductReview>?
    ) : Loaded(product, prices, stats, paginatedReviews)

    data class FailedToLoadProduct(val error: Throwable) : ProductDetailsScreenState()
    data class Failed(val error: Throwable) : ProductDetailsScreenState()
}

/**
 * Transforms a [ProductDetailsScreenState.LoadingProductDetails] into a [ProductDetailsScreenState.LoadedDetails]
 * if all the required data is finally available. Reviews are not available yet.
 */
fun ProductDetailsScreenState.LoadingProductDetails.toLoadedDetailsIfReady(): ProductDetailsScreenState {
    return if (this.prices == null || this.stats == null) {
        this
    } else {
        ProductDetailsScreenState.LoadedDetails(
            product = product,
            prices = prices,
            stats = stats,
            paginatedReviews = null
        )
    }
}

fun ProductDetailsScreenState.LoadedDetails.toLoadingReviews(): ProductDetailsScreenState {
    return ProductDetailsScreenState.LoadingReviews(
        product = product,
        prices = prices,
        stats = stats,
        paginatedReviews = paginatedReviews
    )
}

fun ProductDetailsScreenState.extractProduct(): Product? {
    return when (this) {
        is ProductDetailsScreenState.LoadedProduct -> product
        is ProductDetailsScreenState.LoadingProductDetails -> product
        is ProductDetailsScreenState.Loaded -> product
        else -> null
    }
}

fun ProductDetailsScreenState.extractPrices(): ProductPrices? {
    return when (this) {
        is ProductDetailsScreenState.LoadingProductDetails -> prices
        is ProductDetailsScreenState.Loaded -> prices
        else -> null
    }
}

fun ProductDetailsScreenState.extractStats(): ProductStats? {
    return when (this) {
        is ProductDetailsScreenState.LoadingProductDetails -> stats
        is ProductDetailsScreenState.Loaded -> stats
        else -> null
    }
}

fun ProductDetailsScreenState.extractReviews(): List<ProductReview>? {
    return when (this) {
        is ProductDetailsScreenState.Loaded -> paginatedReviews?.items
        else -> null
    }
}

fun ProductDetailsScreenState.hasMoreReviews(): Boolean {
    return when (this) {
        is ProductDetailsScreenState.Loaded -> paginatedReviews?.hasMore ?: false
        else -> false
    }
}