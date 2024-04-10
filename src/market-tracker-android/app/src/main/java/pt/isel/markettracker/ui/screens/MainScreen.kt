package pt.isel.markettracker.ui.screens

import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import pt.isel.markettracker.navigation.Destination
import pt.isel.markettracker.navigation.ToDestination
import pt.isel.markettracker.navigation.NavBar
import pt.isel.markettracker.ui.screens.list.ListScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreen
import pt.isel.markettracker.ui.theme.Grey

@Composable
fun MainScreen(
    mainScreenViewModel: MainScreenViewModel,
    productsScreenViewModel: ProductsScreenViewModel,
    onProfileRequest: () -> Unit
) {
    val navController = rememberNavController()
    val currentScreen by mainScreenViewModel.currentScreen.collectAsState(initial = Destination.HOME)

    Scaffold(
        containerColor = Grey,
        bottomBar = {
            NavBar(
                Destination.entries,
                Destination.entries.indexOf(currentScreen),
                onItemClick = { route ->
                    mainScreenViewModel.navigateTo(route.ToDestination())
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
               ProfileScreen(
                   onNavigateRequest = onProfileRequest
               )
            }
        }
    }
}