package pt.isel.markettracker.ui.screens.products.topbar

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.SearchBar
import androidx.compose.material3.SearchBarDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.focus.FocusRequester
import androidx.compose.ui.focus.focusRequester
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.theme.Grey

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun EmbeddedSearchBar(
    searchQuery: String,
    active: Boolean,
    onActiveChange: (Boolean) -> Unit,
    onSearch: (String?) -> Unit,
    modifier: Modifier = Modifier,
    onBarcodeScanRequest: () -> Unit
) {
    val previousQueries = remember { mutableStateListOf<String>() }
    val focusRequester = remember { FocusRequester() }
    var currSearchQuery by rememberSaveable { mutableStateOf(searchQuery) }

    LaunchedEffect(active) {
        if (!active) currSearchQuery = searchQuery
    }

    SearchBar(
        modifier = modifier.focusRequester(focusRequester),
        colors = SearchBarDefaults.colors(Grey),
        query = currSearchQuery,
        onQueryChange = {
            currSearchQuery = it
        },
        placeholder = {
            Text(
                text = stringResource(id = R.string.search_hint),
                overflow = TextOverflow.Ellipsis
            )
        },
        onSearch = {
            if (previousQueries.contains(currSearchQuery)) {
                previousQueries.remove(currSearchQuery)
            }
            if (currSearchQuery.isNotEmpty()) previousQueries.add(0, currSearchQuery)
            onSearch(currSearchQuery)
        },
        active = active,
        onActiveChange = onActiveChange,
        leadingIcon = {
            Icon(
                imageVector = Icons.Default.Search,
                contentDescription = "Search"
            )
        },
        trailingIcon = {
            Row(
                modifier = Modifier
                    .fillMaxHeight()
                    .padding(horizontal = 4.dp),
                verticalAlignment = Alignment.CenterVertically,
            ) {
                AnimatedVisibility(searchQuery.isNotEmpty(), enter = fadeIn(), exit = fadeOut()) {
                    IconButton(
                        modifier = Modifier
                            .clip(CircleShape),
                        onClick = {
                            currSearchQuery = ""
                            if (!active) {
                                focusRequester.requestFocus()
                                onActiveChange(true)
                            }
                        }
                    ) {
                        Icon(
                            imageVector = Icons.Default.Close,
                            contentDescription = "Close search",
                            tint = Color.Black
                        )
                    }
                }
                IconButton(
                    onClick = onBarcodeScanRequest,
                    modifier = Modifier
                        .clip(CircleShape)
                ) {
                    Icon(
                        painterResource(R.drawable.barcodeicon),
                        contentDescription = "Scan Bar code",
                        tint = Color.Black
                    )
                }
            }
        }
    ) {
        previousQueries.forEach {
            SearchHistoryItem(
                searchQuery = it,
                onHistoryItemClick = {
                    previousQueries.remove(it)
                    previousQueries.add(0, it)
                    onSearch(it)
                }
            )
        }
    }
}

@Preview
@Composable
fun EmbedSearchBarPreview() {
    EmbeddedSearchBar(
        active = false,
        onActiveChange = { },
        searchQuery = "",
        onSearch = { },
        onBarcodeScanRequest = { }
    )
}