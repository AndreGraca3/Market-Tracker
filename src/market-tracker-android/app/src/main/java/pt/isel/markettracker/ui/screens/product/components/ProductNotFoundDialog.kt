package pt.isel.markettracker.ui.screens.product.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Error
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.Primary400
import pt.isel.markettracker.ui.theme.Primary500

@Composable
fun ProductNotFoundDialog(
    message: String,
    onDismissRequest: () -> Unit
) {
    Dialog(onDismissRequest) {
        Box(
            modifier = Modifier
                .height(450.dp),
            contentAlignment = Alignment.BottomCenter
        ) {
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .fillMaxHeight(0.8f)
                    .background(Color.White, RoundedCornerShape(28.dp))
                    .padding(12.dp),
                contentAlignment = Alignment.Center
            ) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(32.dp)
                ) {
                    Text(
                        text = message,
                        style = MarketTrackerTypography.titleLarge,
                        fontWeight = FontWeight.SemiBold,
                        textAlign = TextAlign.Center
                    )

                    Text(
                        text = "Parece que ainda não temos este produto na nossa base de dados.",
                        style = MarketTrackerTypography.titleMedium,
                        textAlign = TextAlign.Center
                    )

                    Button(
                        onClick = onDismissRequest,
                        modifier = Modifier.fillMaxWidth(),
                        colors = ButtonDefaults.buttonColors(Primary400)
                    ) {
                        Text(text = "Eu compreendo")
                    }
                }
            }
            Icon(
                imageVector = Icons.Default.Error,
                contentDescription = "Close",
                modifier = Modifier
                    .size(150.dp)
                    .background(Primary500, CircleShape)
                    .shadow(12.dp, shape = CircleShape)
                    .align(Alignment.TopCenter)
            )
        }
    }
}

@Preview
@Composable
fun ProductNotFoundDialogPreview() {
    ProductNotFoundDialog(
        message = "Produto não encontrado",
        onDismissRequest = {}
    )
}