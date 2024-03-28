package pt.isel.markettracker.ui.screens.auth

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent

class LoginActivity : ComponentActivity() {
    /*private val vm by viewModels<LoginScreenViewModel> {
        LoginScreenViewModel.factory((application as DependenciesContainer).userService)
    }*/

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            LoginScreen(onBackRequested = { finish() })
        }
    }
}