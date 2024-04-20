package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.foundation.layout.Box
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.vector.ImageVector

@Composable
fun SettingsButton(
    icon: ImageVector,
    onEditRequested: () -> Unit,
    onDeleteRequested: () -> Unit,
    modifier: Modifier
) {
    var expanded by remember { mutableStateOf(false) }

    Box(
        modifier = modifier
    ) {
        IconButton(
            onClick = { expanded = !expanded },
            modifier = modifier
        ) {
            Icon(
                imageVector = icon,
                contentDescription = "settings_icon"
            )
        }

        DropdownMenu(
            expanded = expanded,
            onDismissRequest = { expanded = false },
            modifier = modifier
        ) {
            DropdownMenuItem(
                text = { Text("Edit ✏️") },
                onClick = onEditRequested
            )
            DropdownMenuItem(
                text = { Text("Delete 🗑️") },
                onClick = onDeleteRequested
            )
        }
    }
}