package pt.isel.markettracker.ui.screens

import android.annotation.SuppressLint
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Home
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.ShoppingCart
import androidx.compose.material3.Button
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.navigation.compose.rememberNavController
import pt.isel.markettracker.navigation.NavBar
import pt.isel.markettracker.navigation.NavBarItem
import pt.isel.markettracker.ui.theme.Grey
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun MainScreen(onLoginRequested: () -> Unit) {
    val navController = rememberNavController()

    val navBarItem = listOf(
        NavBarItem(icon = Icons.Default.Home, label = "Home"),
        NavBarItem(icon = Icons.Default.ShoppingCart, label = "Profile"),
        NavBarItem(icon = Icons.Default.Person, label = "Settings")
    )

    MarkettrackerTheme {

        Scaffold(
            containerColor = Grey,
            bottomBar = {
                NavBar(navBarItem)
            }
        ) {
            Text(text = "Hello Muleiro!")
            Button(onClick = onLoginRequested) {
                Text("Login")
            }
        }
    }
}