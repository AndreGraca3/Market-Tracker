package pt.isel.markettracker.ui.screens

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import pt.isel.markettracker.MarketTrackerDependencyProvider
import pt.isel.markettracker.ui.screens.auth.LoginActivity
import pt.isel.markettracker.utils.NavigateAux

class MainActivity : ComponentActivity() {

    private val dependencies by lazy { application as MarketTrackerDependencyProvider }

    // this activity can have a container of all the view models and pass it down?

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        installSplashScreen().setKeepOnScreenCondition {
            // go to data store get token, if token is valid return false to remove splash screen
            // else go to login activity, not sure if its possible to do this here
            false
        }

        setContent {
            MainScreen(
                onLoginRequested = {
                    NavigateAux.navigateTo<LoginActivity>(this)
                }
            )
        }
    }
}