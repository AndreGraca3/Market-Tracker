package pt.isel.markettracker.ui.screens.signup

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.hilt.navigation.compose.hiltViewModel
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class SignUpActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            val vm: SignUpScreenViewModel = hiltViewModel()

            SignUpScreen(
                name = vm.name,
                username = vm.username,
                email = vm.email,
                password = vm.password,
                onNameChange = { vm.name = it },
                onUsernameChange = { vm.username = it },
                onEmailChange = { vm.email = it },
                onPasswordChange = { vm.password = it },
                onCreateAccountRequested = {
                    vm.createUser()
                    finish()
                },
                onBackRequested = { finish() }
            )
        }
    }
}