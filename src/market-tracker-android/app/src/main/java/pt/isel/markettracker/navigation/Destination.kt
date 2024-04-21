package pt.isel.markettracker.navigation

import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Home
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.ShoppingCart
import androidx.compose.ui.graphics.vector.ImageVector

enum class Destination(val icon: ImageVector, val route: String) {
    HOME(
        Icons.Default.Home, "home"
    ),
    LIST(
        Icons.Default.ShoppingCart, "list"
    ),
    PROFILE(
        Icons.Default.Person, "profile"
    );
}

fun String.toDestination(): Destination {
    return Destination.entries.find { it.route == this } ?: Destination.HOME
}
