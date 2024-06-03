package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.width
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.dummy.dummyStoreOffers
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun CompanyPriceCardHeader(productOffer: ProductOffer, modifier: Modifier = Modifier) {
    Column(
        verticalArrangement = Arrangement.spacedBy(4.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        modifier = modifier
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            Text(
                text = "Desde",
                style = MarketTrackerTypography.bodyMedium
            )

            PriceLabel(productOffer.storeOffer.price, false)
        }

        SubcomposeAsyncImage(
            model = productOffer.storeOffer.store.company.logoUrl,
            contentDescription = "company logo",
            contentScale = ContentScale.Fit,
            loading = {
                CircularProgressIndicator(
                    modifier = Modifier
                        .width(16.dp),
                    strokeWidth = 2.dp
                )
            }
        )
    }
}

@Preview
@Composable
fun CompanyPriceCardHeaderPreview() {
    CompanyPriceCardHeader(
        ProductOffer(
            product = dummyProducts.first(),
            storeOffer = dummyStoreOffers.first()
        )
    )
}