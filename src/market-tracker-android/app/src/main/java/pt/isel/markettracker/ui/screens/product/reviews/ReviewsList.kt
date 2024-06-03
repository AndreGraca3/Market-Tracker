package pt.isel.markettracker.ui.screens.product.reviews

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview

@Composable
fun ReviewsList(scrollState: LazyListState, reviews: List<ProductReview>, hasMore: Boolean, loadMoreReviews: () -> Unit) {
    val isItemReachEndScroll by remember {
        derivedStateOf {
            scrollState.layoutInfo.visibleItemsInfo.lastOrNull()?.index ==
                    scrollState.layoutInfo.totalItemsCount - 1
        }
    }

    LaunchedEffect(key1 = isItemReachEndScroll) {
        if (isItemReachEndScroll) {
            loadMoreReviews()
        }
    }

    if (reviews.isEmpty()) {
        Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = Alignment.Center
        ) {
            Image(
                painter = painterResource(id = R.drawable.not_found),
                contentDescription = "No reviews"
            )
        }
        return
    }

    LazyColumn(
        state = scrollState,
        contentPadding = PaddingValues(10.dp),
        verticalArrangement = Arrangement.spacedBy(10.dp),
        modifier = Modifier.padding(bottom = 20.dp)
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