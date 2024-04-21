package pt.isel.markettracker.ui.screens.product.alert

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.widthIn
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowLeft
import androidx.compose.material.icons.automirrored.filled.ArrowRight
import androidx.compose.material.icons.filled.Euro
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.centToEuro

@Composable
fun PriceThresholdAdjuster(
    price: Int,
    currentPriceThreshold: Int,
    onPriceThresholdChange: (Int) -> Unit
) {
    Row(
        modifier = Modifier.fillMaxWidth(),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(14.dp),
    ) {
        CombinedGestureButton(
            imageVector = Icons.AutoMirrored.Filled.ArrowLeft,
            disabled = currentPriceThreshold == 0,
            onClick = { onPriceThresholdChange(currentPriceThreshold - 1) },
            onLongClick = { onPriceThresholdChange(currentPriceThreshold - 10) }
        )

        Box(
            modifier = Modifier
                .weight(1F),
            contentAlignment = Alignment.Center
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
            ) {
                Text(
                    text = currentPriceThreshold.centToEuro(),
                    style = MarketTrackerTypography.titleLarge,
                    textAlign = TextAlign.Center,
                    maxLines = 1,
                    modifier = Modifier
                        .width(70.dp)
                )
                Icon(
                    imageVector = Icons.Default.Euro,
                    contentDescription = "Euro"
                )
            }
        }

        CombinedGestureButton(
            imageVector = Icons.AutoMirrored.Filled.ArrowRight,
            disabled = currentPriceThreshold >= price,
            onClick = { onPriceThresholdChange(currentPriceThreshold + 1) },
            onLongClick = { onPriceThresholdChange(currentPriceThreshold + 10) }
        )
    }
}

@Preview
@Composable
fun PriceThresholdAdjusterPreview() {
    PriceThresholdAdjuster(
        price = 1000,
        currentPriceThreshold = 500,
        onPriceThresholdChange = {}
    )
}