package pt.isel.markettracker.ui.screens.profile

import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable

@Composable
fun ProfileScreen(
    onNavigateRequest: () -> Unit
) {
    Text(text = "This is the profile screen")

    Button(
        onClick = onNavigateRequest
    ) {
        Text("Start Login Activity")
    }
}