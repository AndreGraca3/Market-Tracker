package pt.isel.markettracker.ui.screens

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import com.google.mlkit.vision.barcode.common.Barcode
import com.google.mlkit.vision.codescanner.GmsBarcodeScannerOptions
import com.google.mlkit.vision.codescanner.GmsBarcodeScanning
import dagger.hilt.android.AndroidEntryPoint
import dagger.hilt.android.lifecycle.withCreationCallback
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

    private val vm by viewModels<ProfileScreenViewModel>(
        extrasProducer = {
            defaultViewModelCreationExtras.withCreationCallback<ProfileScreenViewModelFactory> { factory ->
                factory.create(contentResolver)
            }
        }
    )

    @Inject
    lateinit var authRepository: IAuthRepository

    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen().setKeepOnScreenCondition {
            // upload FCM token to server
            false
        }
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                Navigation(
                    profileScreenViewModel = vm,
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
                        val scanner =
                            GmsBarcodeScanning.getClient(this, barcodeScannerOptions)

                        scanner.startScan().addOnSuccessListener {
                            navigateTo<ProductDetailsActivity>(
                                this,
                                ProductDetailsActivity.PRODUCT_ID_EXTRA,
                                ProductIdExtra(it.rawValue ?: "")
                            )
                        }
                    },
                    authRepository = authRepository
                )
            }
        }
    }

    private val barcodeScannerOptions by lazy {
        GmsBarcodeScannerOptions.Builder()
            .setBarcodeFormats(Barcode.FORMAT_EAN_13)
            .setBarcodeFormats(Barcode.FORMAT_EAN_8)
            .build()
    }
}