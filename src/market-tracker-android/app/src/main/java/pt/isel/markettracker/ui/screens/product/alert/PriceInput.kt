package pt.isel.markettracker.ui.screens.product.alert

import android.util.Log
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Euro
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.TextRange
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.TextFieldValue
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.markettracker.R
import pt.isel.markettracker.utils.euroToCent

@Composable
fun PriceInput(maxPrice: Int, onValueChange: (Int) -> Unit) {
    var priceState by remember { mutableStateOf(PriceInputState(maxPrice.toDouble(), "", true)) }

    var textFieldValue by remember {
        mutableStateOf(
            TextFieldValue(
                text = priceState.priceText,
                selection = TextRange.Zero
            )
        )
    }

    Column {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            TextField(
                value = textFieldValue,
                onValueChange = { newTextFieldValue ->
                    val newPriceState = priceState.setNewPrice(newTextFieldValue.text)

                    priceState = newPriceState
                    textFieldValue = newTextFieldValue.copy(
                        text = newPriceState.priceText,
                        selection = TextRange(newPriceState.priceText.length)
                    )

                    onValueChange(newPriceState.getPriceValue().euroToCent())
                },
                placeholder = { Text("0,00", color = Color.Gray) },
                isError = !priceState.isValid,
                keyboardOptions = KeyboardOptions.Default.copy(
                    keyboardType = KeyboardType.Number,
                )
            )
            Icon(
                imageVector = Icons.Default.Euro,
                contentDescription = "Euro"
            )
        }

        AnimatedVisibility(visible = !priceState.isValid) {
            Text(
                text = if (priceState.isInsideBounds) stringResource(id = R.string.invalid_price)
                else "Max: ${priceState.maxPrice}€",
                modifier = Modifier
                    .padding(end = 10.dp),
                fontSize = 16.sp,
                color = Color.Red
            )
        }
    }
}

@Preview
@Composable
fun PriceInputPreview() {
    PriceInput(1000) {}
}