package pt.isel.markettracker.ui.screens

import android.os.Bundle
import android.widget.Toast
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
        } else {
            Toast.makeText(this, "Failed to scan barcode", Toast.LENGTH_SHORT).show()
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen().setKeepOnScreenCondition {
            // upload FCM token to server
            false
        }
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                NavGraph(
                    profileScreenViewModel = profileScreenViewModel,
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
                    onBarcodeScanRequest = {
                        barCodeLauncher.launch(barcodeScannerOptions)
                    },
                    authRepository = authRepository
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
}