package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ProductRatingsRow(
    statsState: IOState<ProductStats>,
    productReviewState: IOState<ProductReview?>,
    onCommunityReviewsRequest: () -> Unit
) {
    Column(
        modifier = Modifier.fillMaxWidth(),
        horizontalAlignment = Alignment.Start,
        verticalArrangement = Arrangement.spacedBy(8.dp),
    ) {
        Text(
            text = stringResource(id = R.string.product_ratings_section_title),
            style = MarketTrackerTypography.bodyMedium
        )

        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.spacedBy(18.dp, Alignment.CenterHorizontally),
            verticalAlignment = Alignment.CenterVertically
        ) {

            CommunityRatingBox(
                productStatsState = statsState,
                onCommunityReviewsRequest = onCommunityReviewsRequest
            )

            UserRatingBox(
                productReviewState = productReviewState,
                onUserRatingRequest = { }
            )
        }
    }
}