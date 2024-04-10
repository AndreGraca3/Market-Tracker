package pt.isel.markettracker.ui.screens.login

import android.util.Log
import android.widget.Toast
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.IntrinsicSize
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Email
import androidx.compose.material.icons.filled.Password
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.credentials.GetCredentialRequest
import com.example.markettracker.R
import com.google.android.libraries.identity.googleid.GetGoogleIdOption
import com.google.android.libraries.identity.googleid.GoogleIdTokenCredential
import com.google.android.libraries.identity.googleid.GoogleIdTokenParsingException
import kotlinx.coroutines.launch
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.mainFont
import java.security.MessageDigest
import java.util.UUID


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
    onCreateAccountRequested: () -> Unit,
    onBackRequested: () -> Unit
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
                MarketTrackerTextField(
                    value = email,
                    onValueChange = onEmailChange,
                    leadingIcon = {
                        Icon(
                            Icons.Default.Email,
                            contentDescription = "Email"
                        )
                    },
                    placeholder = { Text(text = "Email", fontFamily = mainFont) },
                    modifier = Modifier.testTag(LoginEmailInputTag)
                )

                MarketTrackerTextField(
                    value = password,
                    onValueChange = onPasswordChange,
                    leadingIcon = {
                        Icon(
                            Icons.Default.Password,
                            contentDescription = "password"
                        )
                    },
                    placeholder = { Text(text = "Password", fontFamily = mainFont) },
                    isPassword = true,
                    modifier = Modifier.testTag(LoginPasswordInputTag)
                )

                Button(
                    onClick = onLoginRequested
                ) {
                    Text(text = "Login", fontFamily = mainFont)
                }

                Button(
                    onClick = onGoogleSignUpRequested
                ) {
                    Image(
                        painter = painterResource(R.drawable.img),
                        contentDescription = "GoogleIcon",
                        modifier = Modifier.height(IntrinsicSize.Min)
                    )
                }

                GoogleButton()
            }
        }
    }
}

