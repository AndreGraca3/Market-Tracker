package pt.isel.markettracker

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.runtime.mutableStateOf
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.lifecycleScope
import androidx.lifecycle.repeatOnLifecycle
import kotlinx.coroutines.launch
import pt.isel.markettracker.ui.screens.MainScreen
import pt.isel.markettracker.ui.screens.auth.LoginActivity
import pt.isel.markettracker.utils.NavigateAux

class MainActivity : ComponentActivity() {

    private val dependencies by lazy { application as MarketTrackerDependencyProvider }

    // private val isFocus = mutableStateOf(false)

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