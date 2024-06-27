package pt.isel.markettracker.ui.screens.products.filters

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.combinedClickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.FilterAlt
import androidx.compose.material3.Badge
import androidx.compose.material3.BadgedBox
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.Primary500

@OptIn(ExperimentalFoundationApi::class)
@Composable
fun FilterButton(
    enabled: Boolean,
    hasFilters: Boolean,
    onFiltersRequest: () -> Unit,
    onFiltersReset: () -> Unit
) {
    val contentColor = if (enabled) Primary500 else Primary500.copy(alpha = 0.5f)

    BadgedBox(badge = {
        if (hasFilters)
            Badge(containerColor = contentColor, contentColor = contentColor)
    }) {
        Row(
            modifier = Modifier
                .clip(RoundedCornerShape(8.dp))
                .combinedClickable(
                    enabled = enabled,
                    onClick = onFiltersRequest,
                    onLongClick = onFiltersReset
                )
                .padding(6.dp),
            horizontalArrangement = Arrangement.spacedBy(8.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            Icon(
                imageVector = Icons.Default.FilterAlt,
                contentDescription = "Filter",
                tint = contentColor
            )

            Text("Filtros", color = contentColor)
        }
    }
}

@Preview
@Composable
fun FilterButtonPreview() {
    MarkettrackerTheme {
        FilterButton(enabled = true, hasFilters = true, onFiltersRequest = {}, onFiltersReset = {})
    }
}