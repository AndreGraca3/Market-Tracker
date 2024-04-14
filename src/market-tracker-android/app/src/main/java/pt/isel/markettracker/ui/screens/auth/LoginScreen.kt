package pt.isel.markettracker.ui.screens.auth

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontFamily
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun LoginScreen(onBackRequest: () -> Unit) {
    MarkettrackerTheme {
        Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = Alignment.Center
        ) {
            Column {
                Text("This is the login screen")
                Button(onClick = onBackRequest) {
                    Text(text = "Go back to main screen")
                }
                Text(text = "No font specified")
                Text(text = "Explicit main font", fontFamily = mainFont)
                Text(text = "Default font", fontFamily = FontFamily.Default)
            }
        }
    }
}