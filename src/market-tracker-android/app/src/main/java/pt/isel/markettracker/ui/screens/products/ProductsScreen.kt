package pt.isel.markettracker.ui.screens.products

import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable

@Composable
fun ProductsScreen(onLoginRequested: () -> Unit) {
    Text(text = "Products Screen")
    Button(onClick = onLoginRequested) {
        Text(text = "Login")
    }
}