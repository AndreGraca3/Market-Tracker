package pt.isel.markettracker.ui.screens.login.components

import android.app.Activity
import android.content.Intent
import android.util.Log
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.runtime.Composable
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.tasks.Task
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.buttons.ButtonWithImage

@Composable
fun GoogleLoginButton(
    googleSignInHandler: (Task<GoogleSignInAccount>) -> Unit,
    getGoogleLoginIntent: () -> Intent
) {
    val startForResult =
        rememberLauncherForActivityResult(contract = ActivityResultContracts.StartActivityForResult()) {
            val intent = it.data
            if (it.resultCode == Activity.RESULT_OK && intent != null) {
                val task = GoogleSignIn.getSignedInAccountFromIntent(intent)
                googleSignInHandler(task)
            } else {
                Log.e("KueijoBom", "Result Code : ${it.resultCode}")
            }
        }

    ButtonWithImage(
        onClick = { startForResult.launch(getGoogleLoginIntent()) },
        image = R.drawable.google,
        buttonText = "Entrar com o Google"
    )
}