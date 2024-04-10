package pt.isel.markettracker.ui.screens.signup

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.mainFont

const val SignUpScreenTag = "SignUpScreenTag"
const val SignUpNameInputTag = "SignUpNameInputTag"
const val SignUpUsernameInputTag = "SignUpUsernameInputTag"
const val SignUpEmailInputTag = "SignUpEmailInputTag"
const val SignUpPasswordInputTag = "SignUpPasswordInputTag"

@Composable
fun SignUpScreen(
    name: String,
    username: String,
    email: String,
    password: String,
    onNameChange: (String) -> Unit,
    onUsernameChange: (String) -> Unit,
    onEmailChange: (String) -> Unit,
    onPasswordChange: (String) -> Unit,
    onCreateAccountRequested: () -> Unit,
    onBackRequested: () -> Unit
) {
    MarkettrackerTheme {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .testTag(SignUpScreenTag),
            contentAlignment = Alignment.Center,
        ) {
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(24.dp)
            ) {
                MarketTrackerTextField(
                    value = name,
                    onValueChange = onNameChange,
                    leadingIcon = {
                        Text(text = "👤")
                    },
                    placeholder = { Text(text = "Name", fontFamily = mainFont) },
                    modifier = Modifier.testTag(SignUpNameInputTag)
                )

                MarketTrackerTextField(
                    value = username,
                    onValueChange = onUsernameChange,
                    leadingIcon = {
                        Text(text = "👥")
                    },
                    placeholder = { Text(text = "Username", fontFamily = mainFont) },
                    modifier = Modifier.testTag(SignUpUsernameInputTag)
                )

                MarketTrackerTextField(
                    value = email,
                    onValueChange = onEmailChange,
                    leadingIcon = {
                        Text(text = "📧")
                    },
                    placeholder = { Text(text = "Email", fontFamily = mainFont) },
                    modifier = Modifier.testTag(SignUpEmailInputTag)
                )

                MarketTrackerTextField(
                    value = password,
                    onValueChange = onPasswordChange,
                    leadingIcon = {
                        Text(text = "🔑")
                    },
                    placeholder = { Text(text = "Password", fontFamily = mainFont) },
                    isPassword = true,
                    modifier = Modifier.testTag(SignUpPasswordInputTag)
                )

                Button(
                    onClick = onCreateAccountRequested
                ) {
                    Text(text = "Registar", fontFamily = mainFont)
                }

                Button(
                    onClick = onBackRequested
                ) {
                    Text(text = "Cancelar", fontFamily = mainFont)
                }
            }
        }
    }
}

@Preview
@Composable
fun SignUpScreenPreview() {
    SignUpScreen(
        name = "",
        username = "",
        email = "",
        password = "",
        onNameChange = {},
        onUsernameChange = {},
        onEmailChange = {},
        onPasswordChange = {},
        onCreateAccountRequested = {},
        onBackRequested = {}
    )
}