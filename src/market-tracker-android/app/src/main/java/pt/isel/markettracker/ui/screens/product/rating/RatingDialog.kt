package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.wrapContentHeight
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import com.example.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.ui.components.icons.RatingStarsRow
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.Primary400

@Composable
fun RatingDialog(
    dialogOpen: Boolean,
    review: ProductReview?,
    onReviewRequest: (Int, String) -> Unit,
    onDeleteRequest: () -> Unit,
    onDismissRequest: () -> Unit
) {
    if (dialogOpen) {
        var rating by rememberSaveable { mutableIntStateOf(review?.rating ?: 0) }
        var text by rememberSaveable { mutableStateOf(review?.comment ?: "") }

        Dialog(onDismissRequest = onDismissRequest) {
            Card(
                modifier = Modifier
                    .fillMaxWidth()
                    .wrapContentHeight(),
                shape = RoundedCornerShape(16.dp),
            ) {
                Column(
                    modifier = Modifier
                        .padding(21.dp),
                    verticalArrangement = Arrangement.spacedBy(18.dp, Alignment.CenterVertically),
                    horizontalAlignment = Alignment.CenterHorizontally
                ) {
                    Text(
                        text = stringResource(id = R.string.user_rating_section_title),
                        style = MarketTrackerTypography.titleLarge,
                        fontWeight = FontWeight.Bold
                    )

                    RatingStarsRow(rating.toDouble(), onStarClicked = {
                        rating = it
                    })

                    RatingTextField(text, onTextChange = { text = it })

                    if (review != null) {
                        Button(onClick = onDeleteRequest) {
                            Row {
                                Icon(
                                    imageVector = Icons.Default.Delete,
                                    contentDescription = "Delete review"
                                )
                                Text(stringResource(id = R.string.delete_review))
                            }
                        }
                    }

                    Row(
                        horizontalArrangement = Arrangement.spacedBy(
                            10.dp,
                            Alignment.CenterHorizontally
                        )
                    ) {
                        ActionButton(
                            onClick = { onDismissRequest() },
                            text = stringResource(id = R.string.reject)
                        )
                        ActionButton(
                            onClick = { onReviewRequest(rating, text) },
                            text = stringResource(id = R.string.accept),
                            enabled = rating > 0 && (rating != review?.rating || text != review.comment),
                        )
                    }
                }
            }
        }
    }
}

@Composable
private fun RatingTextField(text: String, onTextChange: (String) -> Unit) {
    OutlinedTextField(
        value = text,
        onValueChange = {
            if (it.length <= 255) {
                onTextChange(it)
            }
        },
        minLines = 3,
        maxLines = 4,
        supportingText = {
            Row {
                Text(
                    "${text.length}",
                    color = if (text.length == 255) Color.Red else Color.Black
                )
                Text("/255")
            }
        },
        trailingIcon = {
            AnimatedVisibility(text.isNotEmpty()) {
                IconButton(onClick = { onTextChange("") }) {
                    Icon(
                        imageVector = Icons.Default.Close,
                        contentDescription = "Clear text"
                    )
                }
            }
        },
        placeholder = {
            Text(
                stringResource(id = R.string.describe_product),
                color = Color.Gray
            )
        }
    )
}

@Composable
private fun ActionButton(onClick: () -> Unit, text: String, enabled: Boolean = true) {
    Button(
        onClick = onClick,
        enabled = enabled,
        colors = ButtonDefaults.buttonColors(containerColor = Primary400)
    ) {
        Text(text)
    }
}