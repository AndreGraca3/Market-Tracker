package pt.isel.markettracker.ui.screens

import android.content.Intent
import android.net.Uri
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import com.journeyapps.barcodescanner.ScanContract
import com.journeyapps.barcodescanner.ScanOptions
import dagger.hilt.android.AndroidEntryPoint
import dagger.hilt.android.lifecycle.withCreationCallback
import pt.isel.markettracker.navigation.NavGraph
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.ui.screens.product.ProductDetailsActivity
import pt.isel.markettracker.ui.screens.product.ProductIdExtra
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModel
import pt.isel.markettracker.ui.screens.profile.ProfileScreenViewModelFactory
import pt.isel.markettracker.ui.screens.signup.SignUpActivity
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.utils.navigateTo
import javax.inject.Inject

@AndroidEntryPoint
class MainActivity : ComponentActivity() {

    private val profileScreenViewModel by viewModels<ProfileScreenViewModel>(
        extrasProducer = {
            defaultViewModelCreationExtras.withCreationCallback<ProfileScreenViewModelFactory> { factory ->
                factory.create(contentResolver)
            }
        }
    )

    @Inject
    lateinit var authRepository: IAuthRepository

    private val barCodeLauncher = registerForActivityResult(ScanContract()) { result ->
        if (result.contents != null) {
            navigateTo<ProductDetailsActivity>(
                this,
                ProductDetailsActivity.PRODUCT_ID_EXTRA,
                ProductIdExtra(result.contents ?: "")
            )
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen()
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                NavGraph(
                    onProductClick = {
                        navigateTo<ProductDetailsActivity>(
                            this,
                            ProductDetailsActivity.PRODUCT_ID_EXTRA,
                            ProductIdExtra(it)
                        )
                    },
                    onSignUpRequested = {
                        navigateTo<SignUpActivity>(this)
                    },
                    onForgotPasswordRequested = { onForgotPassword() },
                    onBarcodeScanRequest = {
                        barCodeLauncher.launch(barcodeScannerOptions)
                    },
                    authRepository = authRepository,
                    profileScreenViewModel = profileScreenViewModel
                )
            }
        }
    }

    private val barcodeScannerOptions by lazy {
        ScanOptions()
            .setDesiredBarcodeFormats(ScanOptions.EAN_13, ScanOptions.EAN_8)
            .setPrompt("Escaneie o código de barras do produto")
            .setBeepEnabled(false)
            .setOrientationLocked(false)
    }

    private fun onForgotPassword() {
        val intent = Intent(Intent.ACTION_VIEW, Uri.parse("https://www.youtube.com/watch?v=dQw4w9WgXcQ"))
        startActivity(intent)
    }
}