package pt.isel.markettracker.ui.screens.login

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Email
import androidx.compose.material.icons.filled.Password
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.TextUnit
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.markettracker.R
import pt.isel.markettracker.ui.components.button.ButtonWithImage
import pt.isel.markettracker.ui.components.text.LinesWithElementCentered
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.mainFont


const val LoginScreenTag = "LoginScreenTag"
const val LoginEmailInputTag = "LoginEmailInputTag"
const val LoginPasswordInputTag = "LoginPasswordInputTag"

@Composable
fun LoginScreen(
    email: String,
    password: String,
    onEmailChange: (String) -> Unit,
    onPasswordChange: (String) -> Unit,
    onLoginRequested: () -> Unit,
    onGoogleSignUpRequested: () -> Unit,
    onCreateAccountRequested: () -> Unit
) {
    MarkettrackerTheme {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .testTag(LoginScreenTag),
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
                    onValueChange = onEmailChange,
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

                MarketTrackerTextField(
                    value = password,
                    onValueChange = onPasswordChange,
                    leadingIcon = {
                        Icon(
                            imageVector = Icons.Default.Password,
                            contentDescription = "password"
                        )
                    },
                    placeholder = {
                        Text(text = "Password", fontFamily = mainFont)
                    },
                    isPassword = true,
                    modifier = Modifier.testTag(LoginPasswordInputTag)
                )

                Button(
                    onClick = onLoginRequested
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

                ButtonWithImage(
                    onClick = onGoogleSignUpRequested,
                    image = R.drawable.google,
                    buttonText = "Entrar com o Google"
                )

                ButtonWithImage(
                    onClick = onCreateAccountRequested,
                    image = R.drawable.mt_logo,
                    buttonText = "Criar conta Market Tracker"
                )
            }
        }
    }
}

