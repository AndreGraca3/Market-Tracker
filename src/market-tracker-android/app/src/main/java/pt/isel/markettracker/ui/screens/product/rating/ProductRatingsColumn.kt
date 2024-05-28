package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats

@Composable
fun ProductRatingsColumn(
    productPreferences: ProductPreferences?,
    productStats: ProductStats?,
    onCommunityReviewsRequest: () -> Unit,
    onUserRatingRequest: () -> Unit
) {
    Column(
        modifier = Modifier.fillMaxWidth(),
        horizontalAlignment = Alignment.Start,
        verticalArrangement = Arrangement.spacedBy(8.dp),
    ) {
        Column(
            modifier = Modifier.fillMaxWidth(),
            verticalArrangement = Arrangement.spacedBy(21.dp),
        ) {

            CommunityRatingBox(
                productStats = productStats,
                onCommunityReviewsRequest = onCommunityReviewsRequest
            )

            UserRatingBox(
                preferences = productPreferences,
                onUserRatingRequest = onUserRatingRequest
            )
        }
    }
}