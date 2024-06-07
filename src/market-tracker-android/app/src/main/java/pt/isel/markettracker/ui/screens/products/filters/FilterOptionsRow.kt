package pt.isel.markettracker.ui.screens.products.filters

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.HorizontalDivider
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsSortOption
import pt.isel.markettracker.domain.model.market.inventory.product.filter.resetAll
import pt.isel.markettracker.ui.components.dropdowns.Dropdown

@Composable
fun FilterOptionsRow(
    enabled: Boolean,
    query: ProductsQuery,
    onQueryChange: (ProductsQuery) -> Unit,
    isLoading: Boolean
) {
    val sortOptions = remember {
        ProductsSortOption.entries.map { it.title }
    }

    var isFiltersOpen by rememberSaveable { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(horizontal = 10.dp, vertical = 2.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.SpaceBetween,
            modifier = Modifier
                .fillMaxWidth()
                .padding(vertical = 6.dp)
        ) {
            Box(
                modifier = Modifier.weight(0.5F),
                contentAlignment = Alignment.Center
            ) {
                FilterButton(
                    enabled = enabled,
                    hasFilters = query.hasFiltersApplied,
                    onFiltersRequest = { isFiltersOpen = true }) {
                    onQueryChange(query.copy(filters = query.filters.resetAll()))
                }
            }

            // sort dropdown
            Dropdown(
                enabled = enabled,
                options = sortOptions,
                selected = query.sortOption.title,
                onSelectedChange = {
                    onQueryChange(
                        query.copy(sortOption = ProductsSortOption.fromTitle(it))
                    )
                },
                modifier = Modifier.weight(0.7F)
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