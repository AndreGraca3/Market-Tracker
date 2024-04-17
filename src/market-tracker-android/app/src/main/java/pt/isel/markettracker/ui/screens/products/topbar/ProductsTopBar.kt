package pt.isel.markettracker.ui.screens.products.topbar

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.animateColorAsState
import androidx.compose.animation.core.LinearEasing
import androidx.compose.animation.core.tween
import androidx.compose.animation.slideInHorizontally
import androidx.compose.animation.slideOutHorizontally
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.screens.products.ProductsFilters
import pt.isel.markettracker.ui.theme.Grey
import pt.isel.markettracker.ui.theme.Primary400

@Composable
fun ProductsTopBar(
    searchQuery: String,
    onQueryChange: (String) -> Unit,
    onSearch: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit
) {
    var isSearching by rememberSaveable { mutableStateOf(false) }
    val background by animateColorAsState(
        if (isSearching) Grey else Primary400,
        label = "background"
    )
    val previousQueries = remember { mutableStateListOf<String>() }

    Row(
        modifier = Modifier
            .fillMaxWidth()
            .background(background)
            .padding(8.dp, 8.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(14.dp)
    ) {
        AnimatedVisibility(
            visible = !isSearching, enter = slideInHorizontally(
                initialOffsetX = { -300 },
                animationSpec = tween(
                    durationMillis = 500,
                    easing = LinearEasing
                )
            ), exit = slideOutHorizontally(
                targetOffsetX = { -300 },
                animationSpec = tween(
                    durationMillis = 100,
                    easing = LinearEasing
                )
            )
        ) {
            HeaderLogo()
        }
        EmbeddedSearchBar(
            active = isSearching,
            onActiveChange = { isSearching = it },
            onSearch = {
                isSearching = false
                onSearch(it)
                previousQueries.add(0, it)
            },
            searchQuery = searchQuery,
            onQueryChange = onQueryChange,
            searchHistory = previousQueries,
            onBarcodeScanRequest = onBarcodeScanRequest,
            modifier = Modifier.weight(1f)
        ) {
        }
    }
}

@Preview
@Composable
fun ProductsTopBarPreview() {
    ProductsTopBar(
        searchQuery = "",
        onQueryChange = {},
        onSearch = {},
        onBarcodeScanRequest = {}
    )
}
