package pt.isel.markettracker.ui.screens.login

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import pt.isel.markettracker.MarketTrackerDependencyProvider
import pt.isel.markettracker.ui.screens.signup.SignUpActivity
import pt.isel.markettracker.utils.NavigateAux

class LoginActivity : ComponentActivity() {

    private val vm by viewModels<LoginScreenViewModel> {
        val app = application as MarketTrackerDependencyProvider
        LoginScreenViewModel.factory()
    }

    /*private val nonce = "a0899sd409540s2301203912i312h3h1283h120h3120"

    private val googleIdOption: GetGoogleIdOption = GetGoogleIdOption.Builder()
        .setFilterByAuthorizedAccounts(true)
        .setServerClientId(SERVER_CLIENT_ID)
        .setNonce(nonce)
        .build()

    private val request: GetCredentialRequest = GetCredentialRequest.Builder()
        .addCredentialOption(googleIdOption)
        .build()

    private suspend fun googleSignInHandle() {
        try {
            val result = CredentialManager.create().getCredential(
                request = request,
                context = this.baseContext,
            )
            Log.e("ola", "Result: ")
            handleSignIn(result)
        } catch (e: GetCredentialException) {
            // ignoring exceptions...
        }
    }

    private fun handleSignIn(result: GetCredentialResponse) {
        // Handle the successfully returned credential.
        when (val credential = result.credential) {
            is PublicKeyCredential -> {
                // Share responseJson such as a GetCredentialResponse on your server to
                // validate and authenticate
                /*responseJson =*/
                Log.e("ola", "PublicKeyCredential: ${credential.authenticationResponseJson}")
                credential.authenticationResponseJson
            }

            is PasswordCredential -> {
                // Send ID and password to your server to validate and authenticate.
                val username = credential.id
                val password = credential.password
                Log.e("ola", "PasswordCredential => username: $username and password: $password")
            }

            is CustomCredential -> {
                if (credential.type == GoogleIdTokenCredential.TYPE_GOOGLE_ID_TOKEN_CREDENTIAL) {
                    try {
                        // Use googleIdTokenCredential and extract id to validate and
                        // authenticate on your server.
                        val googleIdTokenCredential = GoogleIdTokenCredential
                            .createFrom(credential.data)
                    } catch (e: GoogleIdTokenParsingException) {
                        Log.e("ola", "Received an invalid google id token response", e)
                    }
                } else {
                    // Catch any unrecognized custom credential type here.
                    Log.e("ola", "Unexpected type of credential")
                }
            }

            else -> {
                // Catch any unrecognized credential type here.
                Log.e("ola", "Unexpected type of credential")
            }
        }
    }*/

    /*
    @Composable
fun GoogleButton() {
    val context = LocalContext.current
    val coroutineScope = rememberCoroutineScope()
    val serverClientId = "317635904868-mgmlu2g27gt43tb00c7i5kfevprerrsn.apps.googleusercontent.com"

    val onClick: () -> Unit = {
        val credentialManager = androidx.credentials.CredentialManager.create(context)

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
                    is androidx.credentials.exceptions.GetCredentialException -> {
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

    Button(onClick = onClick) {
        Text("Google SignIn!")
    }
}
*/
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            LoginScreen(
                email = vm.email,
                password = vm.password,
                onEmailChange = { vm.email = it },
                onPasswordChange = { vm.password = it },
                onLoginRequested = {},
                onGoogleSignUpRequested = {
                    /* runBlocking {
                         googleSignInHandle()
                     }*/
                },
                onCreateAccountRequested = { NavigateAux.navigateTo<SignUpActivity>(this) },
                onBackRequested = { finish() }
            )
        }
    }
}