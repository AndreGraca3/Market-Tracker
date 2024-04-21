package pt.isel.markettracker.ui.screens.products.filters

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.FilterAlt
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

@Composable
fun FilterButton(onFiltersRequest: () -> Unit) {
    Row(
        modifier = Modifier
            .clip(RoundedCornerShape(8.dp))
            .clickable { onFiltersRequest() }
            .padding(8.dp),
        horizontalArrangement = Arrangement.spacedBy(8.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(
            imageVector = Icons.Default.FilterAlt,
            contentDescription = "Filter",
            tint = Primary500
        )

        Text("Filtros", color = Primary500)
    }
}

@Preview
@Composable
fun FilterButtonPreview() {
    MarkettrackerTheme {
        FilterButton(onFiltersRequest = {})
    }
}