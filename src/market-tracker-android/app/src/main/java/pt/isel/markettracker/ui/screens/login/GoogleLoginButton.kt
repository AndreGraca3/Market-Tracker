package pt.isel.markettracker.ui.screens.login

import android.app.Activity
import android.util.Log
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.runtime.Composable
import androidx.compose.ui.platform.LocalContext
import com.example.markettracker.R
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.tasks.Task
import pt.isel.markettracker.ui.components.buttons.ButtonWithImage

@Composable
fun GoogleLoginButton(
    onGoogleLoginRequested: (Task<GoogleSignInAccount>) -> Unit
) {
    val context = LocalContext.current
    val startForResult =
        rememberLauncherForActivityResult(contract = ActivityResultContracts.StartActivityForResult()) {
            if (it.resultCode == Activity.RESULT_OK) {
                val intent = it.data
                if (it.data != null) {
                    val task = GoogleSignIn.getSignedInAccountFromIntent(intent)
                    onGoogleLoginRequested(task)
                }
            } else {
                Log.e("KueijoBom", "Result Code : ${it.resultCode}")
            }
        }

    ButtonWithImage(
        onClick = {
            startForResult.launch(LoginScreenViewModel.getGoogleLoginIntent(context))
        },
        image = R.drawable.google,
        buttonText = "Entrar com o Google"
    )
}