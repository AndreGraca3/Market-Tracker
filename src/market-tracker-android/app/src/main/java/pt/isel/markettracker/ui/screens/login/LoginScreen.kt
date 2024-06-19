package pt.isel.markettracker.ui.screens.login

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun LoginScreen(
    onSignUpRequested: () -> Unit,
    onForgotPasswordClick: () -> Unit,
    onSuggestionRequested: () -> Unit = {},
    loginScreenViewModel: LoginScreenViewModel = hiltViewModel(),
) {
    val loginState by loginScreenViewModel.loginPhase.collectAsState()

    LoginScreenView(
        state = loginState,
        email = loginScreenViewModel.email,
        password = loginScreenViewModel.password,
        onEmailChangeRequested = { loginScreenViewModel.email = it },
        onPasswordChangeRequested = { loginScreenViewModel.password = it },
        onSignUpRequested = onSignUpRequested,
        onLoginRequested = loginScreenViewModel::login,
        onGoogleSignInRequested = loginScreenViewModel::handleGoogleSignInTask,
        onForgotPasswordClick = onForgotPasswordClick,
        onSuggestionRequested = onSuggestionRequested
    )
}