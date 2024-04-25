package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.extractValue
import pt.isel.markettracker.domain.product.ProductStats
import pt.isel.markettracker.ui.components.icons.StarIcon
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun CommunityRatingBox(
    productStatsState: IOState<ProductStats>,
    onCommunityReviewsRequest: () -> Unit
) {
    when (productStatsState) {
        is Loaded -> {
            val stats = productStatsState.extractValue()

            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                modifier = Modifier
                    .clip(RoundedCornerShape(8.dp))
                    .clickable { onCommunityReviewsRequest() }
                    .padding(6.dp)
            ) {
                Text(text = "${stats.averageRating}", style = MarketTrackerTypography.titleLarge)
                Row {
                    repeat(5) { index ->
                        StarIcon(filled = when {
                            index + 1 < stats.averageRating -> true
                            index == stats.averageRating.toInt() -> false
                            else -> null
                        })
                    }
                }
                Text(
                    text = "(${stats.counts.ratings} ${stringResource(id = R.string.global_ratings_name)})",
                    style = MarketTrackerTypography.bodyMedium
                )
            }
        }

        else -> {
            CircularProgressIndicator(
                modifier = Modifier
                    .size(60.dp)
            )
        }
    }
}