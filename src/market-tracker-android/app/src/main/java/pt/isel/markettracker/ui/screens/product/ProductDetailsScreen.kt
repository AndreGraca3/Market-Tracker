package pt.isel.markettracker.ui.screens.product

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.talhafaki.composablesweettoast.util.SweetToastUtil
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.repository.auth.extractAlerts
import pt.isel.markettracker.repository.auth.extractLists
import pt.isel.markettracker.repository.auth.isLoggedIn
import pt.isel.markettracker.ui.screens.product.alert.PriceAlertState
import pt.isel.markettracker.ui.screens.product.components.ProductNotFoundDialog
import pt.isel.markettracker.ui.screens.product.components.ProductTopBar
import pt.isel.markettracker.ui.screens.product.prices.PricesSection
import pt.isel.markettracker.ui.screens.product.rating.ProductRatingsColumn
import pt.isel.markettracker.ui.screens.product.rating.RatingDialog
import pt.isel.markettracker.ui.screens.product.rating.extractPreferences
import pt.isel.markettracker.ui.screens.product.reviews.ReviewsBottomSheet
import pt.isel.markettracker.ui.screens.product.specs.ProductImage
import pt.isel.markettracker.ui.screens.product.specs.ProductSpecs
import pt.isel.markettracker.ui.screens.products.list.AddToListState
import pt.isel.markettracker.ui.screens.products.list.ListsBottomSheet

@Composable
fun ProductDetailsScreen(
    onBackRequest: () -> Unit,
    checkOrRequestNotificationPermission: (() -> Unit) -> Unit,
    onPriceSectionClick: (Int) -> Unit,
    vm: ProductDetailsScreenViewModel,
    authRepository: IAuthRepository
) {
    val screenState by vm.stateFlow.collectAsState()
    val prefsState by vm.prefsStateFlow.collectAsState()
    val priceAlertState by vm.priceAlertStateFlow.collectAsState()

    val authState by authRepository.authState.collectAsState()

    var isReviewsSectionOpen by rememberSaveable { mutableStateOf(false) }
    var isRatingDialogOpen by rememberSaveable { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
    ) {
        ProductTopBar(
            onBackRequest = onBackRequest,
            prefsState.extractPreferences(),
            onFavoriteRequest = if (authState.isLoggedIn()) { isFavorite ->
                vm.submitFavourite(isFavorite)
            } else null
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
            modifier = Modifier.padding(18.dp),
            verticalArrangement = Arrangement.spacedBy(32.dp)
        ) {
            ProductSpecs(product)

            ProductRatingsColumn(
                productPreferences = prefsState.extractPreferences(),
                productStats = screenState.extractStats(),
                onCommunityReviewsRequest = { isReviewsSectionOpen = true },
                isLoggedIn = authState.isLoggedIn(),
                onUserRatingRequest = {
                    isRatingDialogOpen = true
                }
            )

            PricesSection(
                productPrices = screenState.extractPrices(),
                showOptions = authState.isLoggedIn(),
                alerts = authState.extractAlerts(),
                onAlertSet = { id, price ->
                    checkOrRequestNotificationPermission {
                        if (product != null) vm.createAlert(product.id, id, price)
                    }
                },
                onAlertDelete = vm::deleteAlert,
                onAddToListClick = { storeOffer ->
                    if (product != null) vm.selectListToAddProduct(
                        ProductOffer(product, storeOffer)
                    )
                },
                onPriceSectionClick = onPriceSectionClick
            )
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
                review = prefsState.extractPreferences()?.review,
                onReviewRequest = { rating, text ->
                    vm.submitUserRating(product.id, rating, text)
                    isRatingDialogOpen = false
                },
                onDeleteRequest = {
                    isRatingDialogOpen = false
                    vm.deleteReview(product.id)
                },
                onDismissRequest = { isRatingDialogOpen = false }
            )
        }
    }

    when (priceAlertState) {
        is PriceAlertState.Created -> {
            SweetToastUtil.SweetSuccess(
                message = stringResource(id = R.string.alert_set),
                contentAlignment = Alignment.TopCenter,
                padding = PaddingValues(top = 18.dp)
            )
        }

        is PriceAlertState.Deleted -> {
            SweetToastUtil.SweetSuccess(
                message = stringResource(id = R.string.alert_removed),
                contentAlignment = Alignment.TopCenter,
                padding = PaddingValues(top = 18.dp)
            )
        }

        is PriceAlertState.Error -> {
            SweetToastUtil.SweetError(
                message = stringResource(id = R.string.alert_error),
                contentAlignment = Alignment.TopCenter,
                padding = PaddingValues(top = 18.dp)
            )
        }

        else -> {}
    }

    val addToListState by vm.addToListStateFlow.collectAsState()

    when (addToListState) {
        is AddToListState.SelectingList -> {
            ListsBottomSheet(
                shoppingLists = authState.extractLists(),
                onListSelectedClick = vm::addProductToList,
                onDismissRequest = vm::resetAddToListState
            )
        }

        is AddToListState.Success -> {
            SweetToastUtil.SweetSuccess(
                message = stringResource(id = R.string.added_to_list_successfully),
                contentAlignment = Alignment.TopCenter,
                padding = PaddingValues(top = 18.dp)
            )
        }

        is AddToListState.Failed -> {
            SweetToastUtil.SweetError(
                message = stringResource(id = R.string.error_adding_to_list),
                contentAlignment = Alignment.TopCenter,
                padding = PaddingValues(top = 18.dp)
            )
        }

        else -> {}
    }
}