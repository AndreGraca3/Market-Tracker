package pt.isel.markettracker.ui.screens.products.filters

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.HorizontalDivider
import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.screens.ProductsSortOption

@Composable
fun FilterOptions(selectedSort: ProductsSortOption, onFiltersRequest: () -> Unit, onSortRequest: (String) -> Unit) {

    val sortOptions = remember {
        ProductsSortOption.entries.map { it.title }
    }

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
            FilterButton(onFiltersRequest)
            Dropdown(
                options = sortOptions,
                selected = selectedSort.title,
                onSelectedChange = onSortRequest,
                modifier = Modifier.fillMaxWidth(0.8F)
            )
        }   
        HorizontalDivider(modifier = Modifier.fillMaxWidth())
    }
}