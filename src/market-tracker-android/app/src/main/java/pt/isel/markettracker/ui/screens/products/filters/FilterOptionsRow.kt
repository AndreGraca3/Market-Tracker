package pt.isel.markettracker.ui.screens.products.filters

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.HorizontalDivider
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.ui.components.dropdowns.Dropdown
import pt.isel.markettracker.ui.screens.products.ProductsSortOption

@Composable
fun FilterOptionsRow(
    query: ProductsQuery,
    onQueryChange: (ProductsQuery) -> Unit,
    isLoading: Boolean
) {
    val sortOptions = remember {
        ProductsSortOption.entries.map { it.title }
    }

    var isFiltersOpen by remember { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(12.dp, 2.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.SpaceBetween,
            modifier = Modifier
                .fillMaxWidth(0.9F)
                .padding(vertical = 10.dp)
        ) {
            FilterButton(onFiltersRequest = { isFiltersOpen = true })

            // sort dropdown
            Dropdown(
                options = sortOptions,
                selected = query.sortOption.title,
                onSelectedChange = {
                    onQueryChange(
                        query.copy(sortOption = ProductsSortOption.fromTitle(it))
                    )
                },
                modifier = Modifier.fillMaxWidth(0.8F)
            )
        }
        HorizontalDivider(modifier = Modifier.fillMaxWidth())
    }

    FiltersBottomSheet(
        isFiltersOpen = isFiltersOpen,
        onDismissRequest = { isFiltersOpen = false },
        filters = query.filters,
        onFiltersChange = { onQueryChange(query.copy(filters = it)) },
        isLoading = isLoading
    )
}