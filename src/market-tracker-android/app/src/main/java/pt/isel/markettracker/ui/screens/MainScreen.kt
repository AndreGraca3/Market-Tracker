package pt.isel.markettracker.ui.screens

import androidx.compose.animation.AnimatedContentTransitionScope
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.hilt.navigation.compose.hiltViewModel
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

@Composable
fun MainScreen(
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit,
    productsScreenViewModel: ProductsScreenViewModel = hiltViewModel()
) {
    val navController = rememberNavController()
    var selectedIndex by rememberSaveable { mutableIntStateOf(0) }

    navController.addOnDestinationChangedListener { _, destination, _ ->
        selectedIndex = destination.route?.toDestination()?.ordinal ?: 0
    }

    Scaffold(
        contentColor = Color.Black,
        bottomBar = {
            NavBar(
                Destination.entries,
                selectedIndex = selectedIndex,
                onItemClick = { route ->
                    navController.navigate(route) {
                        popUpTo(navController.graph.startDestinationId) {
                            saveState = true
                        }
                        launchSingleTop = true
                        restoreState = true
                    }
                }
            )
        }
    ) { paddingValues ->
        NavHost(
            navController = navController,
            startDestination = Destination.HOME.route,
            modifier = Modifier.padding(paddingValues),
            enterTransition = {
                fadeIn(tween(1000))
            },
            exitTransition = {
                slideOutOfContainer(
                    AnimatedContentTransitionScope.SlideDirection.Start, tween(700)
                )
            },
            popEnterTransition = {
                slideIntoContainer(
                    AnimatedContentTransitionScope.SlideDirection.End, tween(700)
                )
            }) {
            composable(Destination.HOME.route) {
                ProductsScreen(onProductClick, onBarcodeScanRequest, productsScreenViewModel)
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
