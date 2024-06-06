package pt.isel.markettracker.ui.screens.login.components

import androidx.compose.foundation.clickable
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.PanoramaFishEye
import androidx.compose.material.icons.filled.Password
import androidx.compose.material.icons.filled.RemoveRedEye
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.screens.login.LoginPasswordInputTag
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun PasswordTextField(
    value: String,
    onValueChange: (String) -> Unit,
) {

    var isPassword by rememberSaveable { mutableStateOf(false) }

    MarketTrackerTextField(
        value = value,
        onValueChange = onValueChange,
        leadingIcon = {
            Icon(
                imageVector = Icons.Default.Password,
                contentDescription = "leadingIconPassword"
            )
        },
        trailingIcon = {
            Icon(
                imageVector = if (isPassword) Icons.Default.RemoveRedEye else Icons.Default.PanoramaFishEye,
                contentDescription = "trailingIconPassword",
                modifier = Modifier.clickable {
                    isPassword = !isPassword
                }
            )
        },
        placeholder = {
            Text(text = "Password", fontFamily = mainFont)
        },
        isPassword = isPassword,
        modifier = Modifier.testTag(LoginPasswordInputTag)
    )
}