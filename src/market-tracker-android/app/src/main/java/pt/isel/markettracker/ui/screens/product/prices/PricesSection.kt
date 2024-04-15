package pt.isel.markettracker.ui.screens.product.prices

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Warning
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun PricesSection() {
    Column(
        verticalArrangement = Arrangement.spacedBy(22.dp)
    ) {
        Text(
            text = "Comparação de preços",
            fontWeight = FontWeight.SemiBold,
            color = Color.DarkGray,
            style = MarketTrackerTypography.titleLarge
        )

        Row(
            horizontalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            Icon(imageVector = Icons.Default.Warning, contentDescription = "Aviso")
            Text(
                text = "Os preços resgistados podem não estar atualizados em algumas lojas",
                style = MarketTrackerTypography.bodyMedium
            )
        }

        Column(
            verticalArrangement = Arrangement.spacedBy(21.dp),
            modifier = Modifier
                .fillMaxWidth()
        ) {
            (1..10).forEach {
                CompanyRow()
                HorizontalDivider(modifier = Modifier.fillMaxWidth(0.9F))
            }
        }

    }
}