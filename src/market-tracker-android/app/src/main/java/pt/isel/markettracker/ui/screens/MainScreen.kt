package pt.isel.markettracker.ui.screens

import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.navigation.NavDestination.Companion.hierarchy
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import pt.isel.markettracker.navigation.Destination
import pt.isel.markettracker.navigation.NavBar
import pt.isel.markettracker.navigation.toDestination
import pt.isel.markettracker.ui.screens.list.ListScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreen
import pt.isel.markettracker.ui.theme.Grey

@Composable
fun MainScreen(
    productsScreenViewModel: ProductsScreenViewModel
) {
    val navController = rememberNavController()

    Scaffold(
        containerColor = Grey,
        bottomBar = {
            NavBar(
                Destination.entries,
                selectedIndex = navController.currentDestination?.hierarchy?.first()?.route?.toDestination()?.ordinal
                    ?: 0,
                onItemClick = { route ->
                    navController.navigate(route) {
                        popUpTo(navController.graph.startDestinationId)
                        launchSingleTop = true
                    }
                })
        }
    ) {
        NavHost(
            navController = navController,
            startDestination = Destination.HOME.route,
            enterTransition = { fadeIn(tween(400)) },
            exitTransition = { fadeOut(tween(200)) },
            modifier = Modifier.padding(it)
        ) {
            composable(Destination.HOME.route) {
                ProductsScreen(productsScreenViewModel)
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