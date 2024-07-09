package pt.isel.markettracker.ui.screens.login

import android.content.Intent
import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Email
import androidx.compose.material3.Icon
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.tasks.Task
import com.talhafaki.composablesweettoast.util.SweetToastUtil.SweetError
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.buttons.ButtonWithImage
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.components.text.LinesWithElementCentered
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.screens.login.components.GoogleLoginButton
import pt.isel.markettracker.ui.screens.login.components.LoginButton
import pt.isel.markettracker.ui.screens.login.components.PasswordTextField
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.theme.mainFont

const val LoginScreenTag = "LoginScreenTag"
const val LoginEmailInputTag = "LoginEmailInputTag"
const val LoginPasswordInputTag = "LoginPasswordInputTag"

@Composable
fun LoginScreenView(
    state: LoginScreenState,
    email: String,
    password: String,
    onEmailChangeRequested: (String) -> Unit,
    onPasswordChangeRequested: (String) -> Unit,
    onSignUpRequested: () -> Unit,
    onLoginRequested: () -> Unit,
    googleSignInHandler: (Task<GoogleSignInAccount>) -> Unit,
    getGoogleLoginIntent: () -> Intent,
    onSuggestionRequested: () -> Unit,
) {
    Scaffold(
        topBar = {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Red)
                    .padding(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Box(
                    modifier = Modifier.fillMaxWidth()
                ) {
                    HeaderLogo(
                        modifier = Modifier
                            .align(alignment = Alignment.CenterStart)
                            .size(48.dp)
                    )
                    Text(
                        "Login 📝",
                        color = Color.White,
                        fontFamily = mainFont,
                        fontSize = 30.sp,
                        modifier = Modifier
                            .align(alignment = Alignment.Center)
                    )
                }
            }
        }
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
            ) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                    modifier = Modifier.padding(
                        vertical = 24.dp
                    )
                ) {
                    if (state is LoginScreenState.Fail) {
                        SweetError(
                            "Failed to login.\n" +
                                    "Reason: ${state.error.message}",
                            Toast.LENGTH_LONG,
                            contentAlignment = Alignment.Center
                        )
                    }

                    Text(
                        text = "Ao iniciar sessão implica concordância com os termos e condições",
                        fontFamily = mainFont,
                        textAlign = TextAlign.Center,
                        fontSize = 10.sp,
                        maxLines = 1,
                    )

                    MarketTrackerTextField(
                        value = email,
                        onValueChange = onEmailChangeRequested,
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
                }

                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(12.dp)
                ) {
                    PasswordTextField(
                        value = password,
                        onValueChange = onPasswordChangeRequested,
                    )

                    LoginButton(
                        onLoginRequested = onLoginRequested,
                        enabled = state !is LoginScreenState.Loading,
                        loadingContent = {
                            LoadingIcon()
                        }
                    )

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

                    GoogleLoginButton(googleSignInHandler, getGoogleLoginIntent)

                    ButtonWithImage(
                        onClick = onSignUpRequested,
                        image = R.drawable.mt_logo,
                        buttonText = "Criar conta Market Tracker"
                    )

                    ButtonWithImage(
                        onClick = onSuggestionRequested,
                        image = R.drawable.mail,
                        buttonText = "Envia-nos a sua sugestão"
                    )
                }
            }
        }
    }
}

