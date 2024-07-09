package pt.isel.markettracker.ui.screens.list.components

import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextOverflow

@Composable
fun ListNameDisplay(
    listName: String,
) {
    Text(
        text = listName,
        maxLines = 3,
        overflow = TextOverflow.Ellipsis
    )
}