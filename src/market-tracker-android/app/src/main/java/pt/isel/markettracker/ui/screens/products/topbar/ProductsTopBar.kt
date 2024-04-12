package pt.isel.markettracker.ui.screens.products.topbar

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.fadeIn
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.RadioButton
import androidx.compose.material3.SearchBar
import androidx.compose.material3.SearchBarDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.clipToBounds
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.ui.theme.Grey
import pt.isel.markettracker.ui.theme.Primary400
import pt.isel.markettracker.ui.theme.Primary700
import pt.isel.markettracker.utils.bottomBorder

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ProductsTopBar() {

    var active by remember { mutableStateOf(false) }
    var searchQuery by remember { mutableStateOf("") }

    Row(
        modifier = Modifier
            .fillMaxWidth()
            .background(Primary400)
            .padding(0.dp, 6.dp)
            .bottomBorder(1.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        Box(
            contentAlignment = Alignment.Center,
            modifier = Modifier
                .padding(8.dp,0.dp)
                .size(52.dp)
                .background(Color.White, CircleShape)
                .border(
                    width = 2.dp,
                    color = Primary700,
                    shape = CircleShape
                )
        ) {
            Image(
                painter = painterResource(id = R.drawable.mt_logo), contentDescription = "",
                modifier = Modifier
                    .background(Color.White, CircleShape)
                    .padding(5.dp)
            )
        }
        SearchBar(
//            modifier = Modifier.fillMaxWidth(0.8F),
            colors = SearchBarDefaults.colors(Grey),
            query = searchQuery,
            onQueryChange = {
                searchQuery = it
            },
            placeholder = {
                Text("Procurar produto")
            },
            onSearch = {
                searchQuery = it
            },
            active = active,
            onActiveChange = {
                active = it
            },
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
                        .padding(4.dp),
                    horizontalArrangement = Arrangement.spacedBy(8.dp),
                    verticalAlignment = Alignment.CenterVertically,
                ) {
                    AnimatedVisibility(active, enter = fadeIn()) {
                        Box(
                            contentAlignment = Alignment.Center,
                            modifier = Modifier
                                .clip(CircleShape)
                                .clickable { /*TODO*/ }
                        ) {
                            Icon(
                                imageVector = Icons.Default.Close,
                                contentDescription = "Close search",
                                tint = Color.Black,
                                modifier = Modifier.clickable {
                                    if (searchQuery.isNotEmpty()) {
                                        searchQuery = ""
                                    } else {
                                        active = false
                                    }
                                }
                            )
                        }
                    }
                    Box(
                        contentAlignment = Alignment.Center,
                        modifier = Modifier
                            .fillMaxHeight(0.7F)
                            .clip(CircleShape)
                            .clickable { /*TODO*/ }
                    ) {
                        Icon(
                            painterResource(R.drawable.barcodeicon),
                            contentDescription = "Scan Bar code",
                            tint = Color.Black
                        )
                    }
                }
            },
        ) {
        }
        if (!active) {
            RadioButton(selected = false, onClick = { /*TODO*/ })
        }
    }
}