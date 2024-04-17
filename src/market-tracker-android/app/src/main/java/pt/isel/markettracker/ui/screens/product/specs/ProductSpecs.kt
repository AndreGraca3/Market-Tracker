package pt.isel.markettracker.ui.screens.product.specs

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun ProductSpecs(product: ProductInfo?) {
    if (product == null) {
        Column(
            verticalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            Box(modifier = Modifier.fillMaxWidth().height(32.dp).shimmerEffect())
        }
    } else {
        Text(text = product.name, style = MarketTrackerTypography.titleLarge)
    }
}