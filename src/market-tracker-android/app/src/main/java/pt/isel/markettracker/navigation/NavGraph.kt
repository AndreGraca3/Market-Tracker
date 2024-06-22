package pt.isel.markettracker.navigation

import android.app.Activity
import android.content.Intent
import androidx.activity.compose.BackHandler
import androidx.compose.foundation.gestures.detectHorizontalDragGestures
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.WavingHand
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.markettracker.R
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.ui.screens.list.ListScreen
import pt.isel.markettracker.ui.screens.login.LoginScreen
import pt.isel.markettracker.ui.screens.login.LoginScreenViewModel
import pt.isel.markettracker.ui.screens.products.ProductsScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreen
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModel

@Composable
fun NavGraph(
    onProductClick: (String) -> Unit,
    onBarcodeScanRequest: () -> Unit,
    onSignUpRequested: () -> Unit,
    getGoogleLoginIntent: () -> Intent,
    authRepository: IAuthRepository,
    loginScreenViewModel: LoginScreenViewModel,
    profileScreenViewModel: ProfileScreenViewModel,
    productsScreenViewModel: ProductsScreenViewModel
) {
    val navController = rememberNavController()
    var selectedIndex by rememberSaveable { mutableIntStateOf(2) }

    var showExitDialog by rememberSaveable { mutableStateOf(false) }

    navController.addOnDestinationChangedListener { _, destination, _ ->
        selectedIndex = destination.route?.toDestination()?.ordinal ?: 0
    }

    fun changeDestination(destination: String) {
        navController.navigate(destination) {
            popUpTo(navController.graph.startDestinationId) {
                saveState = true
            }
            launchSingleTop = true
            restoreState = true
        }
    }

    BackHandler(enabled = selectedIndex == 0) {
        showExitDialog = true
    }

    Scaffold(
        modifier = Modifier.pointerInput(selectedIndex) {
            detectHorizontalDragGestures { change, dragAmount ->
                change.consume()
                // choosing direction I want to slide
                selectedIndex = if (dragAmount < 0) selectedIndex.inc() else selectedIndex.dec()

                // making sure It doesn't go out of borders
                selectedIndex =
                    if (selectedIndex in 0 until Destination.entries.size) selectedIndex
                    else if (selectedIndex < 0) 0
                    else Destination.entries.indices.last

                val newDestination = Destination.entries[selectedIndex].route
                changeDestination(newDestination)
            }
        },
        contentColor = Color.Black,
        bottomBar = {
            NavBar(
                Destination.entries,
                selectedIndex = selectedIndex,
                onItemClick = { route ->
                    changeDestination(route)
                }
            )
        }
    ) { paddingValues ->
        NavHost(
            navController = navController,
            startDestination = Destination.HOME.route,
            modifier = Modifier.padding(paddingValues)
        ) {
            composable(Destination.HOME.route) {
                ProductsScreen(onProductClick, onBarcodeScanRequest, {
                    changeDestination(Destination.PROFILE.route)
                }, authRepository, productsScreenViewModel)
            }

            composable(Destination.LIST.route) {
                ListScreen()
            }

            composable(Destination.PROFILE.route) {
                val isUserLoggedIn = authRepository.isUserLoggedIn()
                if (isUserLoggedIn) {
                    ProfileScreen(profileScreenViewModel)
                } else {
                    LoginScreen(
                        onSignUpRequested = onSignUpRequested,
                        getGoogleLoginIntent = getGoogleLoginIntent,
                        loginScreenViewModel = loginScreenViewModel
                    )
                }
            }
        }
    }

    val context = LocalContext.current as Activity

    if (showExitDialog) {
        AlertDialog(
            onDismissRequest = { showExitDialog = false },
            title = { Text(text = stringResource(id = R.string.exit_title)) },
            confirmButton = {
                Button(onClick = { context.finish() }) {
                    Text(text = stringResource(id = R.string.accept))
                }
            },
            icon = { Icon(Icons.Filled.WavingHand, null) },
            dismissButton = {
                Button(onClick = { showExitDialog = false }) {
                    Text(text = stringResource(id = R.string.reject))
                }
            }
        )
    }
}