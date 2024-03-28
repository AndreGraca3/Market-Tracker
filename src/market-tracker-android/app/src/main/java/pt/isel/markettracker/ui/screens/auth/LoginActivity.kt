package pt.isel.markettracker.ui.screens.auth

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontFamily
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.mainFont

class LoginActivity : ComponentActivity() {
    /*private val vm by viewModels<LoginScreenViewModel> {
        LoginScreenViewModel.factory((application as DependenciesContainer).userService)
    }*/

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MarkettrackerTheme {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    Column {
                        Text("This is the login screen")
                        Button(onClick = {
                            finish()
                        }) {
                            Text(text = "Go back to main screen")
                        }
                        Text(text = "No font specified")
                        Text(text = "Explicit main font", fontFamily = mainFont)
                        Text(text = "Default font", fontFamily = FontFamily.Default)
                    }
                }
            }
        }
    }
}