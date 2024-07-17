package pt.isel.markettracker.ui.components.buttons

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.Primary400
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun MarketTrackerOutlinedButton(
    modifier: Modifier = Modifier,
    text: String? = null,
    enabled: Boolean = true,
    icon: ImageVector,
    onClick: () -> Unit,
) {
    OutlinedButton(
        modifier = modifier,
        onClick = onClick,
        enabled = enabled,
        colors = ButtonDefaults.buttonColors(Primary400),
        border = BorderStroke(2.dp, Color.Black),
        contentPadding = PaddingValues(4.dp),
    ) {
        Row(
            horizontalArrangement = Arrangement.Center,
            verticalAlignment = Alignment.CenterVertically
        ) {
            if (!text.isNullOrBlank()) {
                Text(
                    text = text,
                    fontFamily = mainFont,
                    color = Color.White
                )
            }
            Icon(
                imageVector = icon,
                contentDescription = null,
                tint = Color.White,
            )
        }
    }
}