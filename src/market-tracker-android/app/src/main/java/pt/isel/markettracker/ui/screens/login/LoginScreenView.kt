package pt.isel.markettracker.ui.screens.login

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Email
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.markettracker.R
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.tasks.Task
import pt.isel.markettracker.ui.components.buttons.ButtonWithImage
import pt.isel.markettracker.ui.components.text.LinesWithElementCentered
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.screens.login.components.GoogleLoginButton
import pt.isel.markettracker.ui.screens.login.components.PasswordTextField
import pt.isel.markettracker.ui.theme.mainFont

const val LoginScreenTag = "LoginScreenTag"
const val LoginEmailInputTag = "LoginEmailInputTag"
const val LoginPasswordInputTag = "LoginPasswordInputTag"

@Composable
fun LoginScreenView(
    state: LoginScreenState,
    onSignUpRequested: () -> Unit,
    onLoginRequested: (String, String) -> Unit,
    onGoogleSignInRequested: (Task<GoogleSignInAccount>) -> Unit
) {
    var email by rememberSaveable { mutableStateOf("") }
    var password by rememberSaveable { mutableStateOf("") }
    Scaffold(
        topBar = {}
    ) { paddingValues ->
        Box(
            modifier = Modifier
                .fillMaxSize()
                .testTag(LoginScreenTag)
                .padding(paddingValues),
            contentAlignment = Alignment.Center
        ) {
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(24.dp)
            ) {
                Text(
                    text = "Login",
                    fontFamily = mainFont,
                    color = Color.Red,
                    fontSize = 24.sp
                )

                MarketTrackerTextField(
                    value = email,
                    onValueChange = { email = it },
                    leadingIcon = {
                        Icon(
                            Icons.Default.Email,
                            contentDescription = "Email"
                        )
                    },
                    placeholder = {
                        Text(text = "Email", fontFamily = mainFont)
                    },
                    modifier = Modifier.testTag(LoginEmailInputTag)
                )

                PasswordTextField(
                    value = password,
                    onValueChange = { password = it},
                )

                Button(
                    onClick = { onLoginRequested(email, password) },
                    enabled = state !is LoginScreenState.Loading
                ) {
                    Text(text = "Login", fontFamily = mainFont)
                }

                LinesWithElementCentered(
                    xOffset = 3,
                    color = Color.LightGray
                ) {
                    Text(
                        text = "ou",
                        modifier = Modifier.weight(0.2f),
                        textAlign = TextAlign.Center,
                        fontFamily = mainFont
                    )
                }

                GoogleLoginButton(
                    onGoogleLoginRequested = onGoogleSignInRequested
                )

                ButtonWithImage(
                    onClick = onSignUpRequested,
                    image = R.drawable.mt_logo,
                    buttonText = "Criar conta Market Tracker"
                )
            }
        }
    }
}

