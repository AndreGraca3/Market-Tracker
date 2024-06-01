package pt.isel.markettracker.ui.screens.products.filters.facets

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material3.Checkbox
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.filter.FacetItem

@Composable
fun <T> CheckboxFacetRow(
    facet: FacetItem<T>,
    enabled: Boolean,
    onClick: (T) -> Unit,
) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .clickable { if (enabled) onClick(facet.id) },
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(8.dp),
    ) {
        Checkbox(checked = facet.isSelected, enabled = enabled, onCheckedChange = {
            onClick(facet.id)
        })

        Text(
            text = facet.name,
            textAlign = TextAlign.Start,
            color = if (enabled) Color.Black else Color.Gray,
        )

        Text(
            text = facet.count.toString(),
            color = Color.Gray,
            textAlign = TextAlign.End,
            modifier = Modifier.weight(1F)
        )
    }
}