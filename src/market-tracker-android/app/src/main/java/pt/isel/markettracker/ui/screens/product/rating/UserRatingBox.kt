package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
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
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.ui.components.icons.StarIcon
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import java.time.LocalDateTime

@Composable
fun UserRatingBox(
    productReviewState: IOState<ProductReview?>,
    onUserRatingRequest: (ProductReview) -> Unit
) {
    when (productReviewState) {
        is Loaded -> {
            val productReview = productReviewState.extractValue()
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(4.dp),
            ) {
                Row {
                    val starSize = 32.dp
                    repeat(5) {
                        if (productReview == null) {
                            StarIcon(
                                filled = null,
                                modifier = Modifier.size(starSize).clickable {
                                    onUserRatingRequest(
                                        ProductReview(
                                            "a",
                                            it + 1,
                                            "sd",
                                            "cl2",
                                            LocalDateTime.now()
                                        )
                                    )
                                }
                            )
                        } else {
                            StarIcon(filled = it + 1 < productReview.rating, modifier = Modifier.size(starSize))
                        }
                    }
                }
                Text(
                    text = stringResource(id = R.string.write_review),
                    style = MarketTrackerTypography.bodyMedium,
                    modifier = Modifier
                        .clip(RoundedCornerShape(8.dp))
                        .clickable {
                            /* TODO: Open review dialog */
                        }
                        .padding(vertical = 4.dp)
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