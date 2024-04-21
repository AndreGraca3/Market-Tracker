package pt.isel.markettracker.ui.screens.product.alert

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.Primary600

@Composable
fun PriceAlertDialog(
    price: Int,
    onAlertSet: (Int) -> Unit,
    onDismissRequest: () -> Unit
) {
    var currentPriceThreshold by rememberSaveable { mutableIntStateOf(price) }

    Dialog(onDismissRequest = {
        currentPriceThreshold = price
        onDismissRequest()
    }) {
        Card(
            modifier = Modifier
                .fillMaxWidth()
                .height(300.dp)
                .padding(14.dp),
            shape = RoundedCornerShape(16.dp),
        ) {
            Column(
                modifier = Modifier
                    .padding(14.dp)
                    .fillMaxSize(),
                verticalArrangement = Arrangement.Center
            ) {
                Column(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(18.dp)
                ) {
                    Text(
                        text = "Nós avisamos-te quando o preço baixar!",
                        style = MarketTrackerTypography.titleMedium,
                        color = Color.DarkGray,
                        fontWeight = FontWeight.Bold,
                        textAlign = TextAlign.Center
                    )

                    PriceThresholdAdjuster(
                        price = price,
                        currentPriceThreshold = currentPriceThreshold,
                        onPriceThresholdChange = { currentPriceThreshold = it }
                    )

                    IconButton(
                        onClick = { onAlertSet(currentPriceThreshold) },
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(8.dp)
                            .border(1.dp, Color.Black, CircleShape)
                            .background(Primary600, CircleShape)
                    ) {
                        Text(
                            text = "Definir Alerta",
                            style = MarketTrackerTypography.titleMedium,
                            color = Color.White,
                            fontWeight = FontWeight.Bold,
                            textAlign = TextAlign.Center
                        )
                    }
                }
            }
        }
    }
}

@Preview
@Composable
fun PriceAlertDialogPreview() {
    PriceAlertDialog(price = 109, onAlertSet = {}, onDismissRequest = {})
}