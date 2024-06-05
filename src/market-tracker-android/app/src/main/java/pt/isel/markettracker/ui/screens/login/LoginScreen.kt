package pt.isel.markettracker.ui.screens.login

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun LoginScreen(
    onSignUpRequested: () -> Unit,
    loginScreenViewModel: LoginScreenViewModel = hiltViewModel()
) {
    val loginState by loginScreenViewModel.loginPhase.collectAsState()

    LoginScreenView(
        state = loginState,
        onSignUpRequested = onSignUpRequested,
        onLoginRequested = loginScreenViewModel::login,
        onGoogleSignInRequested = loginScreenViewModel::handleGoogleSignInTask
    )
}