package pt.isel.markettracker.ui.screens

import android.widget.Toast
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
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.talhafaki.composablesweettoast.util.SweetToastUtil.SweetError
import pt.isel.markettracker.domain.Fail
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.navigation.Destination
import pt.isel.markettracker.navigation.NavBar
import pt.isel.markettracker.navigation.toDestination
import pt.isel.markettracker.ui.screens.list.ListScreen
import pt.isel.markettracker.ui.screens.login.LoginScreen
import pt.isel.markettracker.ui.screens.login.LoginScreenState
import pt.isel.markettracker.ui.screens.login.LoginScreenViewModel
import pt.isel.markettracker.ui.screens.products.ProductsScreen
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreen
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModel

@Composable
fun MainScreen(
    onProductClick: (String) -> Unit,
    onCreateAccountRequested: () -> Unit,
    productsScreenViewModel: ProductsScreenViewModel = hiltViewModel(),
    loginScreenViewModel: LoginScreenViewModel = hiltViewModel(),
    profileScreenViewModel: ProfileScreenViewModel = hiltViewModel()
) {
    val navController = rememberNavController()
    var selectedIndex by rememberSaveable { mutableIntStateOf(0) }

    val loginState by loginScreenViewModel.loginPhase.collectAsState(initial = idle())

    val userFetchingState by profileScreenViewModel.userPhase.collectAsState(initial = idle())

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
                if (loginState is LoginScreenState.Success) {
                    ProfileScreen(
                        user = userFetchingState,
                        onLogoutRequested = {
                            /** WARNING:
                             * I Realized now I Should be using the same viewModel for both
                             * Profile and Login Screens. Meaning the code bellow is not Correct!
                             * **/
                            profileScreenViewModel.logout()
                            loginScreenViewModel.logout()
                        },
                        onEditRequested = {

                        },
                        onChangeAvatarRequested = {
                            launcher.launch("image/*")
                        },
                        onDeleteAccountRequested = {
                            loginScreenViewModel.logout()
                            profileScreenViewModel.deleteAccount()
                        }
                    )
                } else {
                    if (loginState is Fail) {
                        SweetError(
                            (loginState as Fail).exception.message
                                ?: "Credenciais Inválidas ou Conta Inexistente",
                            Toast.LENGTH_LONG,
                            contentAlignment = Alignment.Center
                        )
                    }
                    LoginScreen(
                        email = loginScreenViewModel.email,
                        password = loginScreenViewModel.password,
                        onEmailChange = { loginScreenViewModel.email = it },
                        onPasswordChange = { loginScreenViewModel.password = it },
                        onLoginRequested = {
                            loginScreenViewModel.login()
                            profileScreenViewModel.fetchUser()
                        },
                        onGoogleLoginRequested = loginScreenViewModel::handleGoogleSignInTask,
                        onCreateAccountRequested = onCreateAccountRequested
                    )
                }
            }
        }
    }
}
