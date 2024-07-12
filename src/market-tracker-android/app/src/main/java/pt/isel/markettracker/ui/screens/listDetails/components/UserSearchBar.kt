package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.layout.ColumnScope
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.SearchBar
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextOverflow
import pt.isel.markettracker.R

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun UserSearchBar(
    query: String,
    onQueryChange: (String) -> Unit,
    onSearch: (String) -> Unit,
    active: Boolean,
    modifier: Modifier = Modifier,
    content: @Composable (ColumnScope.() -> Unit) = {},
) {
    SearchBar(
        query = query,
        onQueryChange = onQueryChange,
        onSearch = onSearch,
        active = active,
        onActiveChange = {},
        placeholder = {
            Text(
                text = stringResource(id = R.string.search_user_hint),
                overflow = TextOverflow.Ellipsis
            )
        },
        trailingIcon = {
            Icon(
                imageVector = Icons.Default.Search,
                contentDescription = "Search"
            )
        },
        modifier = modifier,
        content = content
    )
}