package pt.isel.markettracker.ui.screens.login

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.compose.runtime.rememberCoroutineScope
import pt.isel.markettracker.MarketTrackerDependencyProvider
import pt.isel.markettracker.ui.screens.signup.SignUpActivity
import pt.isel.markettracker.utils.NavigateAux

class LoginActivity : ComponentActivity() {

    private val vm by viewModels<LoginScreenViewModel> {
        val app = application as MarketTrackerDependencyProvider
        LoginScreenViewModel.factory(app.tokenService, app.preferencesRepository)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            val auth = Auth(
                context = this.applicationContext,
                coroutineScope = rememberCoroutineScope()
            )

            LoginScreen(
                email = vm.email,
                password = vm.password,
                onEmailChange = { vm.email = it },
                onPasswordChange = { vm.password = it },
                onLoginRequested = {

                },
                onGoogleSignUpRequested = {
                    auth.googleAuth
                },
                onCreateAccountRequested = { NavigateAux.navigateTo<SignUpActivity>(this) },
            )
        }
    }
}