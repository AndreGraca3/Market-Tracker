package pt.isel.markettracker.ui.screens.product.reviews

import android.util.Log
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview

@Composable
fun ReviewsList(reviews: List<ProductReview>, hasMore: Boolean, loadMoreReviews: () -> Unit) {
    val scrollState = rememberLazyListState()
    val isItemReachEndScroll by remember {
        derivedStateOf {
            scrollState.layoutInfo.visibleItemsInfo.lastOrNull()?.index ==
                    scrollState.layoutInfo.totalItemsCount - 1
        }
    }

    LaunchedEffect(key1 = isItemReachEndScroll) {
        if (isItemReachEndScroll) {
            Log.v("Reviews", "fetching more items")
            loadMoreReviews()
        }
    }

    LazyColumn(
        state = scrollState,
        contentPadding = PaddingValues(10.dp),
        verticalArrangement = Arrangement.spacedBy(10.dp),
    ) {
        items(reviews.size) { index ->
            ReviewTile(reviews[index])
        }
        item {
            if (hasMore) {
                Box(
                    modifier = Modifier.fillMaxWidth(),
                    contentAlignment = Alignment.Center
                ) {
                    CircularProgressIndicator(modifier = Modifier.size(30.dp))
                }
            }
        }
    }
}