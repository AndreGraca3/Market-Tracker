package pt.isel.markettracker.ui.screens.product

import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.domain.model.market.price.ProductPrices

sealed class ProductDetailsScreenState {
    data object Idle : ProductDetailsScreenState()
    data object LoadingProduct : ProductDetailsScreenState()
    data class LoadedProduct(val product: Product) : ProductDetailsScreenState()
    data class LoadingProductDetails(
        val product: Product,
        val prices: ProductPrices? = null,
        val stats: ProductStats? = null,
        val preferences: ProductPreferences? = null,
        val alerts: List<PriceAlert>? = null,
    ) : ProductDetailsScreenState()

    abstract class Loaded(
        open val product: Product,
        open val prices: ProductPrices,
        open val stats: ProductStats,
        open val preferences: ProductPreferences,
        open val alerts: List<PriceAlert>,
        open val paginatedReviews: PaginatedResult<ProductReview>?
    ) : ProductDetailsScreenState()

    data class LoadedDetails(
        override val product: Product,
        override val prices: ProductPrices,
        override val stats: ProductStats,
        override val preferences: ProductPreferences,
        override val alerts: List<PriceAlert>,
        override val paginatedReviews: PaginatedResult<ProductReview>?
    ) : Loaded(product, prices, stats, preferences, alerts, paginatedReviews)

    data class LoadingReviews(
        override val product: Product,
        override val prices: ProductPrices,
        override val stats: ProductStats,
        override val preferences: ProductPreferences,
        override val alerts: List<PriceAlert>,
        override val paginatedReviews: PaginatedResult<ProductReview>?
    ) : Loaded(product, prices, stats, preferences, alerts, paginatedReviews)

    data class SubmittingPriceAlert(
        override val product: Product,
        override val prices: ProductPrices,
        override val stats: ProductStats,
        override val preferences: ProductPreferences,
        override val alerts: List<PriceAlert>,
        override val paginatedReviews: PaginatedResult<ProductReview>?
    ) : Loaded(product, prices, stats, preferences, alerts, paginatedReviews)

    data class SubmittingReview(
        override val product: Product,
        override val prices: ProductPrices,
        override val stats: ProductStats,
        override val preferences: ProductPreferences,
        override val alerts: List<PriceAlert>,
        override val paginatedReviews: PaginatedResult<ProductReview>?
    ) : Loaded(product, prices, stats, preferences, alerts, paginatedReviews)

    data class FailedToLoadProduct(val error: Throwable) : ProductDetailsScreenState()
    data class Failed(val error: Throwable) : ProductDetailsScreenState()
}

/**
 * Transforms a [ProductDetailsScreenState.LoadingProductDetails] into a [ProductDetailsScreenState.LoadedDetails]
 * if all the required data is finally available. Reviews are not available yet.
 */
fun ProductDetailsScreenState.LoadingProductDetails.toLoadedDetailsIfReady(): ProductDetailsScreenState {
    return if (this.prices == null || this.stats == null || this.preferences == null || this.alerts == null) {
        this
    } else {
        ProductDetailsScreenState.LoadedDetails(
            product = product,
            prices = prices,
            stats = stats,
            preferences = preferences,
            alerts = alerts,
            paginatedReviews = null
        )
    }
}

fun ProductDetailsScreenState.LoadedDetails.toLoadingReviews(): ProductDetailsScreenState {
    return ProductDetailsScreenState.LoadingReviews(
        product = product,
        prices = prices,
        stats = stats,
        preferences = preferences,
        alerts = alerts,
        paginatedReviews = paginatedReviews
    )
}

fun ProductDetailsScreenState.LoadedDetails.toSubmittingPriceAlert(): ProductDetailsScreenState {
    return ProductDetailsScreenState.SubmittingPriceAlert(
        product = product,
        prices = prices,
        stats = stats,
        preferences = preferences,
        alerts = alerts,
        paginatedReviews = paginatedReviews
    )
}

fun ProductDetailsScreenState.LoadedDetails.toSubmittingReview(): ProductDetailsScreenState {
    return ProductDetailsScreenState.SubmittingReview(
        product = product,
        prices = prices,
        stats = stats,
        preferences = preferences,
        alerts = alerts,
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

fun ProductDetailsScreenState.extractPreferences(): ProductPreferences? {
    return when (this) {
        is ProductDetailsScreenState.LoadingProductDetails -> preferences
        is ProductDetailsScreenState.Loaded -> preferences
        else -> null
    }
}

fun ProductDetailsScreenState.extractAlerts(): List<PriceAlert>? {
    return when (this) {
        is ProductDetailsScreenState.LoadingProductDetails -> alerts
        is ProductDetailsScreenState.Loaded -> alerts
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