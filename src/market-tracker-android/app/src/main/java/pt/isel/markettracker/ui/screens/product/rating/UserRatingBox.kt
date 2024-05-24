package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
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
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.ui.screens.product.reviews.ReviewTile
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun UserRatingBox(
    preferences: ProductPreferences?,
    onUserRatingRequest: () -> Unit
) {
    Column(
        verticalArrangement = Arrangement.spacedBy(4.dp),
        modifier = Modifier.fillMaxWidth()
    ) {
        preferences?.let {
            Text(
                text = stringResource(id = R.string.user_rating_section_title),
                fontWeight = FontWeight.Bold,
                style = MarketTrackerTypography.bodyLarge
            )

            if (preferences.review != null) {
                Box(
                    modifier = Modifier
                        .clip(RoundedCornerShape(8.dp))
                        .clickable { onUserRatingRequest() }
                        .padding(6.dp)
                ) {
                    ReviewTile(preferences.review)
                }
            } else {
                Text(
                    text = stringResource(id = R.string.write_review),
                    textAlign = TextAlign.Center,
                    style = MarketTrackerTypography.bodyMedium,
                    modifier = Modifier
                        .align(Alignment.End)
                        .clip(RoundedCornerShape(8.dp))
                        .clickable { onUserRatingRequest() }
                        .padding(vertical = 4.dp)
                )
            }
        } ?: CircularProgressIndicator(modifier = Modifier.size(60.dp))
    }
}