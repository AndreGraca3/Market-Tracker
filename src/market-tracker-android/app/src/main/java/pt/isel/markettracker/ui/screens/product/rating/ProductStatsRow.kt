package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ProductStatsRow(rating: Double, onCommunityReviewsRequest: () -> Unit) {
    Column(
        modifier = Modifier.fillMaxWidth(),
        horizontalAlignment = Alignment.Start,
        verticalArrangement = Arrangement.spacedBy(8.dp),
    ) {
        Text(text = "Estatísticas do produto", style = MarketTrackerTypography.bodyMedium)

        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceEvenly,
            verticalAlignment = Alignment.CenterVertically
        ) {
            CommunityRatingBox(
                rating = rating,
                onCommunityReviewsRequest = onCommunityReviewsRequest
            )

            ProductMetricsBox(
                favourites = 10,
                ratings = 20,
                lists = 30
            )
        }
    }
}
