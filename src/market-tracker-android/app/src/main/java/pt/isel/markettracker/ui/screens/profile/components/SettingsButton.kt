package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.Logout
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material.icons.filled.Edit
import androidx.compose.material.icons.filled.Logout
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import com.example.markettracker.R

@Composable
fun SettingsButton(
    onEditRequested: () -> Unit,
    onLogoutRequested: () -> Unit,
    modifier: Modifier
) {
    var expanded by rememberSaveable { mutableStateOf(false) }

    Box(
        modifier = modifier
    ) {
        IconButton(
            onClick = { expanded = !expanded },
            modifier = modifier
        ) {
            Icon(
                painter = painterResource(R.drawable.more_vert),
                contentDescription = "settings_icon",
                tint = Color.White
            )
        }

        DropdownMenu(
            expanded = expanded,
            onDismissRequest = { expanded = false },
            modifier = modifier
                .background(color = Color.Red)
        ) {
            DropdownMenuItem(
                text = {
                    Text(
                        text = "Edit",
                        color = Color.White
                    )
                },
                trailingIcon = {
                    Icon(
                        imageVector = Icons.Default.Edit,
                        contentDescription = null,
                        tint = Color.White
                    )
                },
                onClick = onEditRequested
            )
            DropdownMenuItem(
                text = {
                    Text(
                        text = "Logout",
                        color = Color.White
                    )
                },
                trailingIcon = {
                    Icon(
                        imageVector = Icons.AutoMirrored.Filled.Logout,
                        contentDescription = null,
                        tint = Color.White
                    )
                },
                onClick = onLogoutRequested
            )
        }
    }
}