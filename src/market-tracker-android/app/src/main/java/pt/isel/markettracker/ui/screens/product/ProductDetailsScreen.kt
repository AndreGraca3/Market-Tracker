package pt.isel.markettracker.ui.screens.product

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import com.talhafaki.composablesweettoast.util.SweetToastUtil
import pt.isel.markettracker.ui.screens.product.components.ProductNotFoundDialog
import pt.isel.markettracker.ui.screens.product.components.ProductTopBar
import pt.isel.markettracker.ui.screens.product.prices.PricesSection
import pt.isel.markettracker.ui.screens.product.rating.ProductRatingsColumn
import pt.isel.markettracker.ui.screens.product.rating.RatingDialog
import pt.isel.markettracker.ui.screens.product.reviews.ReviewsBottomSheet
import pt.isel.markettracker.ui.screens.product.specs.ProductImage
import pt.isel.markettracker.ui.screens.product.specs.ProductSpecs

@Composable
fun ProductDetailsScreen(
    onBackRequest: () -> Unit,
    vm: ProductDetailsScreenViewModel
) {
    val screenState by vm.stateFlow.collectAsState()

    var isReviewsSectionOpen by remember { mutableStateOf(false) }
    var isRatingDialogOpen by remember { mutableStateOf(false) }
    var isPriceAlertDialogOpen by remember { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
    ) {
        ProductTopBar(
            onBackRequest = onBackRequest,
            screenState.extractPreferences(),
            onFavoriteRequest = {}
        )

        if (screenState is ProductDetailsScreenState.FailedToLoadProduct) {
            ProductNotFoundDialog(
                message = stringResource(id = R.string.product_not_found_title),
                onDismissRequest = onBackRequest
            )
        }

        val product = screenState.extractProduct()
        ProductImage(product?.imageUrl)

        Column(
            modifier = Modifier.padding(24.dp, 18.dp),
            verticalArrangement = Arrangement.spacedBy(32.dp)
        ) {
            ProductSpecs(product)

            ProductRatingsColumn(
                productPreferences = screenState.extractPreferences(),
                productStats = screenState.extractStats(),
                onCommunityReviewsRequest = { isReviewsSectionOpen = true },
                onUserRatingRequest = {
                    isRatingDialogOpen = true
                }
            )

            PricesSection(screenState.extractPrices())
        }

        if (product != null) {
            ReviewsBottomSheet(
                reviewsOpen = isReviewsSectionOpen,
                reviews = screenState.extractReviews(),
                hasMore = screenState.hasMoreReviews(),
                loadMoreReviews = { vm.fetchProductReviews(product.id) },
                onDismissRequest = { isReviewsSectionOpen = false }
            )

            RatingDialog(
                dialogOpen = isRatingDialogOpen,
                review = screenState.extractPreferences()?.review,
                onReviewRequest = { rating, text ->
                    vm.submitUserRating(product.id, rating, text)
                    isRatingDialogOpen = false
                },
                onDismissRequest = { isRatingDialogOpen = false }
            )
        }

        /*screenState.OnAlertsLoaded { alerts ->
            if (isPriceAlertOpen) {
                PriceAlertDialog(
                    price = it.extractValue().priceAlert?.priceThreshold ?: 0,
                    onAlertSet = { isPriceAlertOpen = false },
                    onDismissRequest = { isPriceAlertOpen = false }
                )
            }
        }*/ // TODO: Implement alerts on individual stores
    }

    if (screenState is ProductDetailsScreenState.Failed) {
        SweetToastUtil.SweetError(message = stringResource(id = R.string.loading_error))
    }
}