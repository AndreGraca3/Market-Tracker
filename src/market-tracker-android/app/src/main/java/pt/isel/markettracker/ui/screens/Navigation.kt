package pt.isel.markettracker.ui.screens

import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.animation.EnterTransition
import androidx.compose.animation.ExitTransition
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
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
import pt.isel.markettracker.repository.auth.AuthEvent
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.ui.screens.list.ListScreen
import pt.isel.markettracker.ui.screens.list.ListScreenViewModel
import pt.isel.markettracker.ui.screens.login.LoginScreen
import pt.isel.markettracker.ui.screens.login.LoginScreenViewModel
import pt.isel.markettracker.ui.screens.products.ProductsScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreen
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModel

@Composable
fun Navigation(
    onProductClick: (String) -> Unit,
    onListClick: (Int) -> Unit,
    onBarcodeScanRequest: () -> Unit,
    onSignUpRequested: () -> Unit,
    authRepository: IAuthRepository,
    productsScreenViewModel: ProductsScreenViewModel = hiltViewModel(),
    listScreenViewModel: ListScreenViewModel = hiltViewModel(),
    loginScreenViewModel: LoginScreenViewModel = hiltViewModel(),
    profileScreenViewModel: ProfileScreenViewModel = hiltViewModel()
) {
    val navController = rememberNavController()
    var selectedIndex by rememberSaveable { mutableIntStateOf(0) }

    val authState by authRepository.authState.collectAsState()

    val launcher =
        rememberLauncherForActivityResult(
            contract = ActivityResultContracts.GetContent(),
            onResult = { contentPath ->
                if (contentPath != null) {
                    profileScreenViewModel.avatarPath = contentPath
                    profileScreenViewModel.updateUser()
                }
            }
        )

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
            enterTransition = { EnterTransition.None },
            exitTransition = { ExitTransition.None }
        ) {
            composable(Destination.HOME.route) {
                ProductsScreen(onProductClick, onBarcodeScanRequest, productsScreenViewModel)
            }

            composable(Destination.LIST.route) {
                ListScreen(onListClick, listScreenViewModel)
            }

            composable(Destination.PROFILE.route) {
                when (authState) {
                    is AuthEvent.Login -> {
                        ProfileScreen(
                            onChangeAvatarRequested = {
                                launcher.launch("image/*")
                            },
                            profileScreenViewModel = profileScreenViewModel
                        )
                    }

                    else -> {
                        LoginScreen(
                            onSignUpRequested = onSignUpRequested,
                            loginScreenViewModel = loginScreenViewModel
                        )
                    }
                }
            }
        }
    }
}