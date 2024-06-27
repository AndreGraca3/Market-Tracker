package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
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
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.ui.components.icons.RatingStarsRow
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun CommunityRatingBox(
    productStats: ProductStats?,
    onCommunityReviewsRequest: () -> Unit
) {
    Column(
        verticalArrangement = Arrangement.spacedBy(4.dp),
        modifier = Modifier.fillMaxWidth()
    ) {
        productStats?.let {
            Text(
                text = stringResource(id = R.string.community_ratings_section_title),
                fontWeight = FontWeight.Bold,
                style = MarketTrackerTypography.bodyLarge
            )
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                modifier = Modifier
                    .align(Alignment.CenterHorizontally)
                    .clip(RoundedCornerShape(8.dp))
                    .clickable { onCommunityReviewsRequest() }
                    .padding(6.dp)
            ) {
                Text(
                    text = "${productStats.averageRating}",
                    style = MarketTrackerTypography.titleLarge
                )
                RatingStarsRow(rating = productStats.averageRating)
                Text(
                    text = "(${productStats.counts.ratings} ${stringResource(id = R.string.global_ratings_name)})",
                    style = MarketTrackerTypography.bodyMedium
                )
            }
        } ?: CircularProgressIndicator(modifier = Modifier.size(60.dp))
    }
}