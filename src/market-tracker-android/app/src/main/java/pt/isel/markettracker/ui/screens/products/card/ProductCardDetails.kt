package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Badge
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.ui.components.buttons.AddToListButton
import pt.isel.markettracker.ui.components.icons.RatingStarsRow
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ProductCardDetails(
    productOffer: ProductOffer,
    onAddToListClick: () -> Unit
) {
    val promotion = productOffer.storeOffer.price.promotion

    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(8.dp),
        modifier = Modifier.padding(12.dp, 8.dp)
    ) {
        Box(
            contentAlignment = Alignment.Center,
            modifier = Modifier
                .fillMaxWidth()
                .fillMaxHeight(.3F)
        ) {
            LoadableImage(
                url = productOffer.product.imageUrl,
                contentDescription = productOffer.product.name,
                modifier = Modifier.fillMaxSize()
            )
            Box(
                contentAlignment = Alignment.TopEnd,
                modifier = Modifier.fillMaxSize()
            ) {
                if (promotion != null)
                    Badge(
                        containerColor = Color.Red,
                    ) {
                        Text(
                            "- ${promotion.percentage}%",
                            style = MarketTrackerTypography.bodySmall,
                            fontWeight = FontWeight.Bold,
                            color = Color.White,
                            modifier = Modifier.padding(4.dp)
                        )
                    }
            }
        }
        ProductCardSpecs(product = productOffer.product)

        RatingStarsRow(rating = productOffer.product.rating)

        Column(
            horizontalAlignment = Alignment.End,
            verticalArrangement = Arrangement.spacedBy(4.dp),
            modifier = Modifier.fillMaxSize(),
        ) {
            CompanyPriceCardHeader(productOffer, modifier = Modifier.weight(1F))
            AddToListButton(
                onClick = {onAddToListClick()},
                modifier = Modifier
                    .fillMaxWidth()
                    .weight(0.5F)
            )
        }
    }
}