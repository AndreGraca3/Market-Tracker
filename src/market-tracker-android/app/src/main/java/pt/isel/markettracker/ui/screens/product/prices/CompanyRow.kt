package pt.isel.markettracker.ui.screens.product.prices

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.ui.screens.product.stores.StoresBottomSheet
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun CompanyRow() {
    var showCompanyStores by rememberSaveable { mutableStateOf(false) }

    Row(
        modifier = Modifier
            .fillMaxWidth(),
        horizontalArrangement = Arrangement.spacedBy(14.dp),
    ) {
        Column(
            modifier = Modifier
                .fillMaxWidth(0.7F),
            verticalArrangement = Arrangement.spacedBy(8.dp),
        ) {
            SubcomposeAsyncImage(
                model = dummyProducts.first().lowestPriceStore.company.logoUrl,
                loading = {
                    CircularProgressIndicator()
                },
                contentDescription = "Company Logo",
                modifier = Modifier
                    .fillMaxWidth(0.5F)
                    .height(50.dp),
                contentScale = ContentScale.Fit
            )

            Text(
                text = dummyProducts.first().lowestPriceStore.storeName,
                modifier = Modifier
                    .clip(RoundedCornerShape(6.dp))
                    .clickable {
                        showCompanyStores = true
                    }
                    .padding(4.dp),
                style = MarketTrackerTypography.bodyMedium,
                fontWeight = FontWeight.SemiBold,
                color = Color.Gray,
                maxLines = 1,
                overflow = TextOverflow.Ellipsis
            )
        }

        CompanyPriceBox(price = dummyProducts.first().lowestPriceStore.price)
    }
    StoresBottomSheet(
        showStores = showCompanyStores,
        onDismissRequest = { showCompanyStores = false })

}

@Preview
@Composable
fun PriceRowPreview() {
    CompanyRow()
}