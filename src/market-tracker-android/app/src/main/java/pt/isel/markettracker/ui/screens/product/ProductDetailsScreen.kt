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
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.exceptionOrNull
import pt.isel.markettracker.domain.extractValue
import pt.isel.markettracker.domain.getOrNull
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.domain.toResultSuccess
import pt.isel.markettracker.ui.screens.product.alert.PriceAlertDialog
import pt.isel.markettracker.ui.screens.product.components.ProductNotFoundDialog
import pt.isel.markettracker.ui.screens.product.components.ProductTopBar
import pt.isel.markettracker.ui.screens.product.prices.PricesSection
import pt.isel.markettracker.ui.screens.product.rating.ProductRatingsRow
import pt.isel.markettracker.ui.screens.product.reviews.ReviewsBottomSheet
import pt.isel.markettracker.ui.screens.product.specs.ProductImage
import pt.isel.markettracker.ui.screens.product.specs.ProductSpecs

@Composable
fun ProductDetailsScreen(
    onBackRequest: () -> Unit,
    vm: ProductDetailsScreenViewModel
) {
    val productState by vm.product.collectAsState()
    val preferencesState by vm.preferences.collectAsState()
    val alertsState by vm.alerts.collectAsState()
    val pricesState by vm.prices.collectAsState()
    val statsState by vm.stats.collectAsState()

    var isReviewsSectionOpen by remember { mutableStateOf(false) }
    var isPriceAlertOpen by remember { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
    ) {
        ProductTopBar(
            onBackRequest = onBackRequest,
            preferencesState = preferencesState,
            onAlertRequest = { isPriceAlertOpen = true },
            onFavoriteRequest = {}
        )

        productState.exceptionOrNull()?.let {
            ProductNotFoundDialog(
                message = stringResource(id = R.string.product_not_found_title),
                onDismissRequest = onBackRequest
            )
        }

        val product = productState.getOrNull()

        ProductImage(product?.imageUrl)

        Column(
            modifier = Modifier.padding(24.dp, 18.dp),
            verticalArrangement = Arrangement.spacedBy(32.dp)
        ) {
            ProductSpecs(product)

            ProductRatingsRow(
                statsState = statsState,
                productReviewState = when (val loadedPrefs = preferencesState) {
                    is Loaded -> loaded(loadedPrefs.extractValue().review.toResultSuccess())
                    else -> loading()
                },
                onCommunityReviewsRequest = { isReviewsSectionOpen = true }
            )

            PricesSection(pricesState)
        }

        ReviewsBottomSheet(
            isReviewsSectionOpen,
            onDismissRequest = { isReviewsSectionOpen = false }
        )

        preferencesState.let {
            if (it is Loaded && isPriceAlertOpen) {
                PriceAlertDialog(
                    price = it.extractValue().priceAlert?.priceThreshold ?: 0,
                    onAlertSet = { isPriceAlertOpen = false },
                    onDismissRequest = { isPriceAlertOpen = false }
                )
            }
        }
    }
}