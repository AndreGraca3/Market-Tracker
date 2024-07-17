package pt.isel.markettracker.ui.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxScope
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo

@Composable
fun MarketTrackerHeader(
    content: @Composable (BoxScope.() -> Unit),
) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .background(Color.Red)
            .padding(10.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(14.dp)
    ) {
        Box(
            modifier = Modifier.fillMaxWidth()
        ) {
            HeaderLogo(
                modifier = Modifier
                    .align(alignment = Alignment.CenterStart)
                    .size(48.dp)
            )
            content()
        }
    }
}