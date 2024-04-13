package pt.isel.markettracker.ui.screens.products.topbar

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.layout.ColumnScope
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
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.ui.theme.Grey

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun EmbeddedSearchBar(
    searchQuery: String,
    active: Boolean,
    onActiveChange: (Boolean) -> Unit,
    onQueryChange: (String) -> Unit,
    onSearch: (String) -> Unit,
    modifier: Modifier = Modifier,
    searchHistory: List<String> = emptyList(),
    content: @Composable (ColumnScope.() -> Unit)
) {
    SearchBar(
        modifier = modifier,
        colors = SearchBarDefaults.colors(Grey),
        query = searchQuery,
        onQueryChange = onQueryChange,
        placeholder = {
            Text("Procurar produto")
        },
        onSearch = onSearch,
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
                    .padding(4.dp, 0.dp),
                verticalAlignment = Alignment.CenterVertically,
            ) {
                AnimatedVisibility(searchQuery.isNotEmpty(), enter = fadeIn(), exit = fadeOut()) {
                    IconButton(
                        modifier = Modifier
                            .clip(CircleShape),
                        onClick = {
                            onQueryChange("")
                            if (active) {
                                onActiveChange(false)
                            } else {
                                onSearch(searchQuery)
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
                    onClick = { /*TODO*/ },
                    modifier = Modifier
                        .clip(CircleShape)
                        .padding(12.dp)
                ) {
                    Icon(
                        painterResource(R.drawable.barcodeicon),
                        contentDescription = "Scan Bar code",
                        tint = Color.Black
                    )
                }
            }
        },
        content = content
    )
}

@Preview
@Composable
fun EmbedSearchBarPreview() {
    EmbeddedSearchBar(
        active = false,
        onActiveChange = { },
        searchQuery = "",
        onQueryChange = { },
        onSearch = { },
        content = {
        }
    )
}