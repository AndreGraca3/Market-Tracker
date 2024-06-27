package pt.isel.markettracker.ui.screens.product.reviews

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ModalBottomSheet
import androidx.compose.material3.rememberModalBottomSheetState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.ui.components.sheets.CustomDragHandle

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ReviewsBottomSheet(
    reviewsOpen: Boolean,
    reviews: List<ProductReview>?,
    hasMore: Boolean,
    loadMoreReviews: () -> Unit,
    onDismissRequest: () -> Unit
) {
    val sheetState = rememberModalBottomSheetState(skipPartiallyExpanded = true)
    val scrollState = rememberLazyListState()

    if (reviewsOpen) {
        ModalBottomSheet(
            modifier = Modifier.fillMaxHeight(0.7F),
            onDismissRequest = onDismissRequest,
            sheetState = sheetState,
            dragHandle = {
                CustomDragHandle(
                    title = "Reviews",
                    onDismissRequest = onDismissRequest
                )
            }
        ) {
            Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = if (reviews == null) Alignment.Center else Alignment.TopCenter
            ) {
                reviews?.let {
                    ReviewsList(
                        scrollState = scrollState,
                        reviews = it,
                        hasMore = hasMore,
                        loadMoreReviews = loadMoreReviews
                    )
                } ?: LoadingReviewsIndicator(loadMoreReviews)
            }
        }
    }
}

@Composable
fun LoadingReviewsIndicator(loadMoreReviews: () -> Unit) {
    LaunchedEffect(Unit) {
        // Load first batch of reviews
        // doesn't run twice because this composable is removed from composition after
        loadMoreReviews()
    }
    CircularProgressIndicator()
}