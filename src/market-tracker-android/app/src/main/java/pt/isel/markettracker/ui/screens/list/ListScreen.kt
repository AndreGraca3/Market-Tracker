package pt.isel.markettracker.ui.screens.list

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable

@Composable
fun ListScreen() {
    Column(
        modifier = androidx.compose.ui.Modifier.verticalScroll(rememberScrollState())
    ) {
        (1..100).forEach {
            Text("This is the list screen")
        }
    }
}