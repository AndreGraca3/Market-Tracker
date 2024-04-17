package pt.isel.markettracker.ui.screens.product.alert

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.gestures.detectTapGestures
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Icon
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.Primary600

@Composable
fun CombinedGestureButton(
    imageVector: ImageVector,
    disabled: Boolean = false,
    onClick: () -> Unit,
    onLongClick: () -> Unit = {}
) {
    Icon(
        imageVector = imageVector,
        contentDescription = null,
        tint = Color(0xFFf57c00),
        modifier = Modifier
            .size(60.dp)
            .border(1.dp, Color.Black, CircleShape)
            .background(if (disabled) Color.Gray else Primary600, CircleShape)
            .pointerInput(onClick) {
                detectTapGestures(
                    onLongPress = { if (!disabled) onLongClick() },
                    onTap = { if (!disabled) onClick() }
                )
            }
    )
}
