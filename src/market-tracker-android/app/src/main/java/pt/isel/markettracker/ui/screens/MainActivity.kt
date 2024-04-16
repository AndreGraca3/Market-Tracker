package pt.isel.markettracker.ui.screens

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import pt.isel.markettracker.MarketTrackerDependencyProvider
import pt.isel.markettracker.ui.screens.login.LoginScreenViewModel
import dagger.hilt.android.AndroidEntryPoint
import pt.isel.markettracker.ui.screens.product.ProductDetailsActivity
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModel
import pt.isel.markettracker.ui.screens.signup.SignUpActivity
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.utils.NavigateAux

@AndroidEntryPoint
class MainActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen().setKeepOnScreenCondition {
            // upload FCM token to server
            false
        }
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                MainScreen(
                    onProductClick = {
                        NavigateAux.navigateTo<ProductDetailsActivity>(this)
                    }
                )
            }
        }
    }
}