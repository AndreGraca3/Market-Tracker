package pt.isel.markettracker.ui.screens

import android.annotation.SuppressLint
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import pt.isel.markettracker.navigation.Destination
import pt.isel.markettracker.navigation.NavBar
import pt.isel.markettracker.ui.screens.list.ListScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreen
import pt.isel.markettracker.ui.screens.profile.ProfileScreen
import pt.isel.markettracker.ui.theme.Grey
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun MainScreen(onLoginRequested: () -> Unit) {
    val navController = rememberNavController()

    MarkettrackerTheme {
        Scaffold(
            containerColor = Grey,
            bottomBar = {
                NavBar(Destination.entries, onItemClick = { route ->
                    navController.navigate(
                        route,
                    )
                })
            }
        ) {
            NavHost(
                navController = navController,
                startDestination = Destination.HOME.route,
                enterTransition = { fadeIn(tween(400)) },
                exitTransition = { fadeOut(tween(200)) }
            ) {
                composable(Destination.HOME.route) {
                    ProductsScreen(onLoginRequested)
                }

                composable(Destination.LIST.route) {
                    ListScreen()
                }

                composable(Destination.PROFILE.route) {
                    ProfileScreen()
                }
            }
        }
    }
}