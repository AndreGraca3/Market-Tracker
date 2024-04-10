package pt.isel.markettracker.ui.screens.signup

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import pt.isel.markettracker.MarketTrackerDependencyProvider

class SignUpActivity : ComponentActivity() {

    private val vm by viewModels<SignUpScreenViewModel> {
        val app = (application as MarketTrackerDependencyProvider)
        SignUpScreenViewModel.factory(app.userService)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            SignUpScreen(
                name = vm.name,
                username = vm.username,
                email = vm.email,
                password = vm.password,
                onNameChange = { vm.name = it },
                onUsernameChange = { vm.username = it },
                onEmailChange = { vm.email = it },
                onPasswordChange = { vm.password = it },
                onCreateAccountRequested = {},
                onBackRequested = { finish() }
            )
        }
    }
}