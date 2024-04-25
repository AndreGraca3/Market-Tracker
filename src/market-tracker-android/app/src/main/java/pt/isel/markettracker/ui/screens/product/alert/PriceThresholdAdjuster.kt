package pt.isel.markettracker.ui.screens.product.alert

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowLeft
import androidx.compose.material.icons.automirrored.filled.ArrowRight
import androidx.compose.material.icons.filled.Euro
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.centToEuro

@Composable
fun PriceThresholdAdjuster(
    currentPriceThreshold: Int?,
    onPriceThresholdChange: (Int) -> Unit
) {
    Row(
        modifier = Modifier.fillMaxWidth(),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(14.dp),
    ) {
        Box(
            modifier = Modifier
                .weight(1F),
            contentAlignment = Alignment.Center
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
            ) {
                TextField(
                    value = currentPriceThreshold?.centToEuro() ?: "",
                    onValueChange = {
                        val price = it.toIntOrNull()
                        if (price != null) {
                            onPriceThresholdChange(price)
                        }
                    },
                    placeholder = {
                        Text(
                            text = "0.00",
                            style = MarketTrackerTypography.titleLarge,
                            textAlign = TextAlign.Center
                        )
                    },
                    textStyle = MarketTrackerTypography.titleLarge,
                    colors = TextFieldDefaults.colors(
                        focusedContainerColor = Color.Transparent,
                        unfocusedContainerColor = Color.Transparent
                    ),
                    keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                    singleLine = true
                )
                Icon(
                    imageVector = Icons.Default.Euro,
                    contentDescription = "Euro"
                )
            }
        }
    }
}

@Preview
@Composable
fun PriceThresholdAdjusterPreview() {
    PriceThresholdAdjuster(
        currentPriceThreshold = 500,
        onPriceThresholdChange = {}
    )
}