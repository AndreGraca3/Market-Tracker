package pt.isel.markettracker.ui.screens.product.alert

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import com.example.markettracker.R
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun PriceAlertDialog(
    showAlertDialog: Boolean,
    price: Int,
    onAlertSet: (Int) -> Unit,
    onDismissRequest: () -> Unit
) {
    var currentPriceThreshold by rememberSaveable { mutableIntStateOf(price) }

    if (!showAlertDialog) return
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
                        text = stringResource(id = R.string.price_alert_title),
                        style = MarketTrackerTypography.titleMedium,
                        color = Color.DarkGray,
                        fontWeight = FontWeight.Bold,
                        textAlign = TextAlign.Center
                    )

                    PriceInput(maxPrice = 500, onValueChange = { currentPriceThreshold = it })

                    Button(
                        onClick = { onAlertSet(currentPriceThreshold) },
                        modifier = Modifier
                            .fillMaxWidth()
                    ) {
                        Text(
                            text = stringResource(id = R.string.set_alert),
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
    PriceAlertDialog(
        showAlertDialog = true, price = 109, onAlertSet = {}, onDismissRequest = {})
}