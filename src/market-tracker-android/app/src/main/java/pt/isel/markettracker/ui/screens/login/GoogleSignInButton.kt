package pt.isel.markettracker.ui.screens.login

import android.content.Context
import android.util.Log
import android.widget.Toast
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.credentials.CredentialManager
import androidx.credentials.GetCredentialRequest
import androidx.credentials.exceptions.GetCredentialException
import com.google.android.libraries.identity.googleid.GetGoogleIdOption
import com.google.android.libraries.identity.googleid.GoogleIdTokenCredential
import com.google.android.libraries.identity.googleid.GoogleIdTokenParsingException
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.launch
import pt.isel.markettracker.ui.theme.mainFont
import java.security.MessageDigest
import java.util.UUID

@Composable
fun ButtonWithImage(
    onClick: () -> Unit = {},
    image: Int,
    buttonText: String
) {
    Button(
        onClick = onClick,
        modifier = Modifier
            .fillMaxWidth(0.8F)
            .shadow(0.dp),
        elevation = ButtonDefaults.elevatedButtonElevation(
            defaultElevation = 0.dp,
            pressedElevation = 0.dp,
            hoveredElevation = 0.dp,
            focusedElevation = 0.dp
        ),
        shape = RoundedCornerShape(28.dp),
        contentPadding = PaddingValues(15.dp),
        colors = ButtonDefaults.buttonColors(Color.White),
        border = BorderStroke(1.dp, Color.LightGray)
    ) {
        Box(
            modifier = Modifier
                .fillMaxWidth(),
            contentAlignment = Alignment.Center
        ) {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .align(Alignment.CenterStart)
            ) {
                Spacer(modifier = Modifier.width(4.dp))
                Icon(
                    painter = painterResource(id = image),
                    modifier = Modifier
                        .size(18.dp),
                    contentDescription = "drawable_icons",
                    tint = Color.Unspecified
                )
            }
            Text(
                modifier = Modifier.align(Alignment.Center),
                text = buttonText,
                textAlign = TextAlign.Center,
                fontFamily = mainFont
            )
        }
    }
}

class Auth(
    private val context: Context,
    private val coroutineScope: CoroutineScope
) {
    // val context = LocalContext.current
    // val coroutineScope = rememberCoroutineScope()
    private val serverClientId =
        "317635904868-mgmlu2g27gt43tb00c7i5kfevprerrsn.apps.googleusercontent.com"

    val googleAuth: () -> Unit = {
        val credentialManager = CredentialManager.create(context)

        val rawNonceBytes = UUID.randomUUID().toString().toByteArray()
        val digest = MessageDigest.getInstance("SHA-256").digest(rawNonceBytes)
        val hashedNonce = digest.fold("") { str, it -> str + "%02x".format(it) }

        val googleIdOption: GetGoogleIdOption = GetGoogleIdOption.Builder()
            .setFilterByAuthorizedAccounts(false)
            .setServerClientId(serverClientId)
            .setNonce(hashedNonce)
            .build()

        val request: GetCredentialRequest = GetCredentialRequest.Builder()
            .addCredentialOption(googleIdOption)
            .build()

        coroutineScope.launch {
            try {
                val result = credentialManager.getCredential(
                    request = request,
                    context = context
                )

                val credential = result.credential

                Log.e("Ola", "Credential: ${result.credential}")


                Log.e("Ola", "googleIdTokenCredential: $GoogleIdTokenCredential")
                Log.e(
                    "Ola",
                    "TYPE_GOOGLE_ID_TOKEN_CREDENTIAL: ${GoogleIdTokenCredential.TYPE_GOOGLE_ID_TOKEN_CREDENTIAL}"
                )
                Log.e(
                    "Ola",
                    "TYPE_GOOGLE_ID_TOKEN_SIWG_CREDENTIAL: ${GoogleIdTokenCredential.TYPE_GOOGLE_ID_TOKEN_SIWG_CREDENTIAL}"
                )
                Log.e(
                    "Ola",
                    "googleIdTokenCredential: ${GoogleIdTokenCredential.BUNDLE_KEY_GOOGLE_ID_TOKEN_SUBTYPE}"
                )
                val googleIdTokenCredential = GoogleIdTokenCredential
                    .createFrom(credential.data)

                Log.e("Ola", "googleIdTokenCredential: $googleIdTokenCredential")

                Log.d("Ola", "Display Name: ${googleIdTokenCredential.displayName}")
                Log.d("Ola", "Family Name: ${googleIdTokenCredential.familyName}")
                Log.d("Ola", "id: ${googleIdTokenCredential.id}")
                Log.d("Ola", "Given Name: ${googleIdTokenCredential.givenName}")
                Log.d("Ola", "Phone Number: ${googleIdTokenCredential.phoneNumber}")
                googleIdTokenCredential.idToken.split(".").forEach { Log.d("Ola", it) }
            } catch (e: Exception) {
                when (e) {
                    is GetCredentialException -> {
                        Log.e("Debug", "Type: ${e.type}")
                        Log.e("Debug", "Error Message: ${e.errorMessage}")
                        Log.e("Debug", "Localized Message: ${e.localizedMessage}")
                        Log.e("Debug", "Cause: ${e.cause}")
                        Log.e("Debug", "StackTrace: ${e.stackTrace}")
                        Log.e("Debug", "Message: ${e.message}")
                        Toast.makeText(context, e.message, Toast.LENGTH_LONG).show()
                    }

                    is GoogleIdTokenParsingException -> {
                        Log.e("Debug", "Localized Message: ${e.localizedMessage}")
                        Log.e("Debug", "Cause: ${e.cause}")
                        Log.e("Debug", "StackTrace: ${e.stackTrace}")
                        Log.e("Debug", "Message: ${e.message}")
                        Toast.makeText(context, e.message, Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }
}