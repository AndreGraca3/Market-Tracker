package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Star
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun CommunityRatingBox(rating: Double, onCommunityReviewsRequest: () -> Unit) {
    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        modifier = Modifier
            .clip(RoundedCornerShape(8.dp))
            .clickable { onCommunityReviewsRequest() }
            .padding(6.dp)
    ) {
        Text(text = "$rating", style = MarketTrackerTypography.titleLarge)
        Row {
            (1..rating.toInt()).forEach {
                Icon(imageVector = Icons.Default.Star, contentDescription = "Star")
            }
        }
    }
}