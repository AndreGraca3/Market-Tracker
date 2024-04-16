package pt.isel.markettracker.ui.screens

import androidx.compose.animation.EnterTransition
import androidx.compose.animation.ExitTransition
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
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.navigation.Destination
import pt.isel.markettracker.navigation.NavBar
import pt.isel.markettracker.navigation.ToDestination
import pt.isel.markettracker.navigation.toDestination
import pt.isel.markettracker.ui.screens.list.ListScreen
import pt.isel.markettracker.ui.screens.login.LoginScreen
import pt.isel.markettracker.ui.screens.login.LoginScreenViewModel
import pt.isel.markettracker.ui.screens.products.ProductsScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreen
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModel
import pt.isel.markettracker.ui.theme.Grey

@Composable
fun MainScreen(
    onProductClick: (String) -> Unit,
    productsScreenViewModel: ProductsScreenViewModel = hiltViewModel()
) {
    val navController = rememberNavController()
    var selectedIndex by rememberSaveable { mutableIntStateOf(0) }

    val loginState by loginScreenViewModel.loginPhase.collectAsState(initial = idle())

    Scaffold(
        contentColor = Color.Black,
        bottomBar = {
            NavBar(
                Destination.entries,
                selectedIndex = selectedIndex,
                onItemClick = { route ->
                    selectedIndex = route.toDestination().ordinal
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
            enterTransition = { EnterTransition.None },
            exitTransition = { ExitTransition.None }
        ) {
            composable(Destination.HOME.route) {
                ProductsScreen(onProductClick, productsScreenViewModel)
            }

            composable(Destination.LIST.route) {
                ListScreen()
            }

            composable(Destination.PROFILE.route) {
                // if logged show profile screen
                if (loginState is Loaded) {
                    ProfileScreen(
                        user = profileScreenViewModel.user
                    )
                } else {
                    LoginScreen(
                        email = loginScreenViewModel.email,
                        password = loginScreenViewModel.password,
                        onEmailChange = { loginScreenViewModel.email = it },
                        onPasswordChange = { loginScreenViewModel.password = it },
                        onLoginRequested = {
                            loginScreenViewModel.login()
                        },
                        onGoogleSignUpRequested = {

                        },
                        onCreateAccountRequested = onCreateAccountRequested
                    )
                }
            }
        }
    }
}
