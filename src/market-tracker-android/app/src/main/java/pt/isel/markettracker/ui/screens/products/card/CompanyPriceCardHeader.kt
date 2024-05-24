package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.dummy.dummyCompanyPrices
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun CompanyPriceCardHeader(productOffer: ProductOffer) {
    Column(
        horizontalAlignment = Alignment.End,
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(4.dp),
            modifier = Modifier.align(Alignment.End)
        ) {
            Text(
                text = "Desde",
                style = MarketTrackerTypography.bodyMedium
            )

            PriceLabel(productOffer.storeOffer.priceData)
        }

        CompanyHeader(productOffer.storeOffer.store.company.logoUrl)
    }
}

@Preview
@Composable
fun CompanyPriceCardHeaderPreview() {
    CompanyPriceCardHeader(
        ProductOffer(
            product = dummyProducts.first(),
            storeOffer = dummyCompanyPrices.first().storeOffers.first(),
            true
        )
    )
}