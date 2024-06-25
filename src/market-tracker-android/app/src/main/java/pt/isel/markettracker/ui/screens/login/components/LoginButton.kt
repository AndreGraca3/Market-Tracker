package pt.isel.markettracker.ui.screens.login.components

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxScope
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.login.LoginScreenState
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun LoginButton(
    onLoginRequested: () -> Unit,
    enabled: Boolean,
    loadingContent: @Composable (BoxScope.() -> Unit)
) {
    Button(
        onClick = onLoginRequested,
        enabled = enabled,
        modifier = Modifier
            .size(width = 100.dp, height = 40.dp)
    ) {
        if (!enabled) {
            Box(
                modifier = Modifier
                    .fillMaxSize(),
                contentAlignment = Alignment.Center
            ) {
                loadingContent()
            }
        } else {
            Text(text = "Login", fontFamily = mainFont)
        }
    }
}