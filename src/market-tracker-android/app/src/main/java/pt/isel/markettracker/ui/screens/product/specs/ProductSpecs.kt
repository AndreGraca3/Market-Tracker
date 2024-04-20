package pt.isel.markettracker.ui.screens.product.specs

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.Primary600
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun ProductSpecs(product: ProductInfo?) {
    Column(
        verticalArrangement = Arrangement.spacedBy(4.dp),
    ) {
        if (product == null) {
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(32.dp)
                    .shimmerEffect()
            )
        } else {
            Text(
                text = product.brand,
                style = MarketTrackerTypography.titleMedium,
                fontWeight = FontWeight.Bold,
                color = Primary600
            )
            Text(
                text = product.name,
                style = MarketTrackerTypography.titleLarge,
                fontWeight = FontWeight.Bold
            )
        }
    }
}