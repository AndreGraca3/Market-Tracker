package pt.isel.markettracker.ui.screens

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import pt.isel.markettracker.MarketTrackerDependencyProvider
import pt.isel.markettracker.ui.screens.product.ProductDetailsActivity
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.utils.NavigateAux

class MainActivity : ComponentActivity() {

    /*
    private val mainScreenViewModel by viewModels<MainScreenViewModel> {
        val app = (application as MarketTrackerDependencyProvider)
        MainScreenViewModel.factory()
    }
    */

    private val productsScreenViewModel by viewModels<ProductsScreenViewModel> {
        val app = (application as MarketTrackerDependencyProvider)
        ProductsScreenViewModel.factory()
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
                    productsScreenViewModel = productsScreenViewModel,
                    // this will have so many parameters as we keep adding more logic to the screens
                    onProductClick = {
                        NavigateAux.navigateTo<ProductDetailsActivity>(this)
                    }
                )
            }
        }
    }
}