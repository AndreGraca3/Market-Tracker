package pt.isel.markettracker.ui.theme

import androidx.compose.foundation.isSystemInDarkTheme
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color

private val LightColorScheme = lightColorScheme(
    primary = Primary,
    background = Color.White,
    onBackground = Color.Black,
    surface = Grey,
    surfaceContainer = Grey,
    surfaceTint = Color.Black,
    onSurface = Color.Black,
)

@Composable
fun MarkettrackerTheme(
    content: @Composable () -> Unit
) {
    MaterialTheme(
        colorScheme = LightColorScheme,
        typography = MarketTrackerTypography,
        content = content
    )
}