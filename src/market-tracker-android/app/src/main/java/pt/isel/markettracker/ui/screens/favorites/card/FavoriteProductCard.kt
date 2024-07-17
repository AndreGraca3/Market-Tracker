package pt.isel.markettracker.ui.screens.favorites.card

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductItem
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.Primary600

@Composable
fun FavoriteProductCard(
    product: ProductItem,
    onRemoveFromFavoritesRequested: () -> Unit,
) {
    val shape = RoundedCornerShape(8.dp)
    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier
            .size(width = 350.dp, 130.dp)
    ) {
        Card(
            modifier = Modifier
                .fillMaxSize()
                .padding(2.dp)
                .border(2.dp, Color.Black.copy(.6F), shape),
            colors = CardDefaults.cardColors(Color.White)
        ) {
            Row(
                horizontalArrangement = Arrangement.spacedBy(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                modifier = Modifier
                    .fillMaxSize()
                    .padding(14.dp, 8.dp)
            ) {
                Box(
                    contentAlignment = Alignment.Center,
                    modifier = Modifier
                        .fillMaxHeight()
                        .fillMaxWidth(.2F)
                ) {
                    LoadableImage(
                        url = product.imageUrl,
                        contentDescription = product.name,
                        modifier = Modifier.fillMaxSize()
                    )
                }

                Box(
                    contentAlignment = Alignment.CenterStart,
                    modifier = Modifier
                        .fillMaxHeight()
                        .fillMaxWidth(.6F)
                ) {
                    Column(
                        verticalArrangement = Arrangement.spacedBy(6.dp),
                    ) {
                        Text(
                            text = product.brandName,
                            style = MarketTrackerTypography.labelMedium,
                            color = Primary600,
                            overflow = TextOverflow.Ellipsis,
                            maxLines = 1,
                            textAlign = TextAlign.Left,
                            fontWeight = FontWeight.Bold
                        )

                        Column(
                            modifier = Modifier.fillMaxWidth(),
                            verticalArrangement = Arrangement.spacedBy(2.dp),
                        ) {
                            Text(
                                text = product.name,
                                style = MarketTrackerTypography.bodyMedium,
                                fontWeight = FontWeight.Bold,
                                overflow = TextOverflow.Ellipsis,
                                maxLines = 2,
                            )
                        }
                    }
                }

                Box(
                    modifier = Modifier
                        .fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    IconButton(
                        onClick = onRemoveFromFavoritesRequested,
                        modifier = Modifier
                            .size(80.dp)
                            .border(2.dp, Color.Black, shape = CircleShape)
                            .background(color = Color.Red, shape = CircleShape),
                    ) {
                        Icon(
                            painter = painterResource(R.drawable.heart_minus),
                            contentDescription = "remove_from_favorites_button",
                            tint = Color.White
                        )
                    }
                }
            }
        }
    }
}