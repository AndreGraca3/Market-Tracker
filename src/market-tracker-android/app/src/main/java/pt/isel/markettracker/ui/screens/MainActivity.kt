package pt.isel.markettracker.ui.screens

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import pt.isel.markettracker.MarketTrackerDependencyProvider
import pt.isel.markettracker.ui.screens.login.LoginScreenViewModel
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModel
import pt.isel.markettracker.ui.screens.signup.SignUpActivity
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.utils.NavigateAux

class MainActivity : ComponentActivity() {

    private val mainScreenViewModel by viewModels<MainScreenViewModel> {
        val app = (application as MarketTrackerDependencyProvider)
        MainScreenViewModel.factory(app.preferencesRepository)
    }

    private val productsScreenViewModel by viewModels<ProductsScreenViewModel> {
        (application as MarketTrackerDependencyProvider)
        ProductsScreenViewModel.factory()
    }

    private val loginScreenViewModel by viewModels<LoginScreenViewModel> {
        val app = (application as MarketTrackerDependencyProvider)
        LoginScreenViewModel.factory(app.tokenService, app.preferencesRepository)
    }

    private val profileScreenViewModel by viewModels<ProfileScreenViewModel> {
        val app = (application as MarketTrackerDependencyProvider)
        ProfileScreenViewModel.factory()
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen().setKeepOnScreenCondition {
            // upload FCM token to server
            false
        }
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                MainScreen(
                    mainScreenViewModel = mainScreenViewModel,
                    productsScreenViewModel = productsScreenViewModel,
                    loginScreenViewModel = loginScreenViewModel,
                    profileScreenViewModel = profileScreenViewModel,
                    onCreateAccountRequested = {
                        NavigateAux.navigateTo<SignUpActivity>(this)
                    }
                )
            }
        }
    }
}