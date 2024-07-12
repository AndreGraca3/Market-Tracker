package pt.isel.markettracker.ui.screens

import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.util.Log
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.core.content.ContextCompat
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import androidx.lifecycle.lifecycleScope
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.journeyapps.barcodescanner.ScanContract
import com.journeyapps.barcodescanner.ScanOptions
import dagger.hilt.android.AndroidEntryPoint
import dagger.hilt.android.lifecycle.withCreationCallback
import kotlinx.coroutines.launch
import pt.isel.markettracker.R
import pt.isel.markettracker.navigation.NavGraph
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.ui.screens.list.ListScreenViewModel
import pt.isel.markettracker.ui.screens.listDetails.ListDetailsActivity
import pt.isel.markettracker.ui.screens.listDetails.ListIdExtra
import pt.isel.markettracker.ui.screens.login.LoginScreenState
import pt.isel.markettracker.ui.screens.login.LoginScreenViewModel
import pt.isel.markettracker.ui.screens.product.ProductDetailsActivity
import pt.isel.markettracker.ui.screens.product.ProductIdExtra
import pt.isel.markettracker.ui.screens.products.ProductsScreenViewModel
import pt.isel.markettracker.ui.screens.products.list.AddToListState
import pt.isel.markettracker.ui.screens.profile.ProfileScreenState
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

    private val productsScreenViewModel by viewModels<ProductsScreenViewModel>()
    private val loginScreenViewModel by viewModels<LoginScreenViewModel>()
    private val listScreenViewModel by viewModels<ListScreenViewModel>()

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
        super.onCreate(savedInstanceState)
        installSplashScreen()
        productsScreenViewModel.fetchProducts(false)
        lifecycleScope.launch {
            val token = authRepository.getToken()
            if (token != null) {
                profileScreenViewModel.fetchUser()
                if (profileScreenViewModel.clientFetchingFlow.value is ProfileScreenState.Fail) {
                    profileScreenViewModel.resetToIdle()
                }
            }
        }

        lifecycleScope.launch {
            productsScreenViewModel.addToListStateFlow.collect {
                if (it is AddToListState.Done) productsScreenViewModel.resetAddToListState()
            }
        }

        lifecycleScope.launch {
            loginScreenViewModel.loginPhase.collect { loginState ->
                Log.v("User", "LoginState is $loginState")
                if (loginState is LoginScreenState.Success) profileScreenViewModel.fetchUser()
            }
        }

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
                    onListClick = {
                        navigateTo<ListDetailsActivity>(
                            this,
                            ListDetailsActivity.LIST_PRODUCT_ID_EXTRA,
                            ListIdExtra(it)
                        )
                    },
                    onSignUpRequested = {
                        navigateTo<SignUpActivity>(this)
                    },
                    getGoogleLoginIntent = { getGoogleLoginIntent(this) },
                    onBarcodeScanRequest = {
                        barCodeLauncher.launch(barcodeScannerOptions)
                    },
                    authRepository = authRepository,
                    loginScreenViewModel = loginScreenViewModel,
                    profileScreenViewModel = profileScreenViewModel,
                    productsScreenViewModel = productsScreenViewModel,
                    listScreenViewModel = listScreenViewModel
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

    private fun getGoogleLoginIntent(ctx: Context): Intent {
        val options = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
            .requestIdToken(ContextCompat.getString(ctx, R.string.GOOGLE_CLIENT_ID))
            .requestProfile()
            .requestEmail()
            .build()
        val client = GoogleSignIn.getClient(ctx, options)
        return client.signInIntent
    }
}