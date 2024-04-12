package pt.isel.markettracker.ui.screens.login

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.RowScope
import androidx.compose.foundation.layout.absoluteOffset
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.mainFont


/**
 * @param xOffset x axis offset, from the side of the screen. Applies in both lines, but for the left line is applied negatively.
 * @param color the color applied to both lines.
 * @param content composable element to be displayed between the lines.
 */
@Composable
fun LinesWithElementCentered(
    xOffset: Int,
    color: Color,
    content: @Composable (RowScope.() -> Unit)
) {
    Row(
        modifier = Modifier.fillMaxWidth(),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Box(
            modifier = Modifier
                .height(2.dp)
                .weight(1F)
                .absoluteOffset(x = (xOffset).dp)
                .background(color) // 0xFFFD785B
        )
        content()
        Box(
            modifier = Modifier
                .height(2.dp)
                .weight(1F)
                .absoluteOffset(x = (-xOffset).dp)
                .background(Color.LightGray) // 0xFFFD785B
        )
    }
}