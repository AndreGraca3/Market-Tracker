package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.Fail
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.extractValue
import pt.isel.markettracker.domain.product.ProductStats
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun ProductStatsRow(statsState: IOState<ProductStats>, onCommunityReviewsRequest: () -> Unit) {
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

            when (statsState) {
                is Loaded -> {
                    val stats = statsState.extractValue()

                    CommunityRatingBox(
                        rating = stats.averageRating,
                        onCommunityReviewsRequest = onCommunityReviewsRequest
                    )

                    ProductMetricsBox(
                        favourites = stats.counts.favourites,
                        ratings = stats.counts.ratings,
                        lists = stats.counts.lists
                    )
                }

                is Fail -> {
                    Text(
                        text = statsState.exception.message ?: "Erro ao carregar estatísticas",
                        style = MarketTrackerTypography.bodyMedium
                    )
                }

                else -> {
                    repeat(2) {
                        Box(
                            modifier = Modifier
                                .size(110.dp, 80.dp)
                                .clip(RoundedCornerShape(12.dp))
                                .shimmerEffect()
                        )
                    }
                }
            }
        }
    }
}
