package pt.isel.markettracker.ui.screens.list

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.Primary900
import pt.isel.markettracker.utils.advanceShadow

data class ListDetails(val name: String, val isArchived: Boolean)

@Composable
fun ListScreen(onListClick: (ListDetails) -> Unit) {
    val lists = listOf(
        ListDetails("List 1", false),
        ListDetails("List 2", true))
    val shape = RoundedCornerShape(8.dp)
    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp),
        modifier = Modifier.verticalScroll(rememberScrollState())
    ) {
        lists.forEach { list ->
            Box(
                modifier = Modifier
                    .clickable { onListClick(list) }
                    .background(Color.LightGray)
                    .padding(2.dp)
                    .border(2.dp, Color.Black.copy(.6F), shape)
                    .advanceShadow(Primary900, blurRadius = 24.dp)
                    .height(100.dp)
                    .fillMaxSize()
            ) {
                Text("List Name: ${list.name}, Archived: ${list.isArchived}")
            }
        }
    }
}