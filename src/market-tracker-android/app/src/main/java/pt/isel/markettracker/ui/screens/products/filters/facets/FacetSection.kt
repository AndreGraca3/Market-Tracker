package pt.isel.markettracker.ui.screens.products.filters.facets

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.filter.FacetItem

@Composable
fun <T> FacetSection(
    facets: List<FacetItem<T>>,
    title: String,
    onFacetsReset: () -> Unit,
    itemContent: @Composable (FacetItem<T>) -> Unit
) {
    Column(
        modifier = Modifier.fillMaxWidth()
    ) {
        Row {
            Text(
                text = title,
                textAlign = TextAlign.Start,
                modifier = Modifier.weight(1F)
            )
            Icon(
                imageVector = Icons.Filled.Close,
                contentDescription = "Reset Facets",
                modifier = Modifier
                    .clip(CircleShape)
                    .clickable {
                        onFacetsReset()
                    }
                    .padding(4.dp)
            )
        }

        HorizontalDivider(modifier = Modifier.fillMaxWidth())

        Column {
            facets.forEach { facet ->
                itemContent(facet)
            }
        }
    }
}