package pt.isel.markettracker.ui.screens.product

import android.os.Bundle
import android.os.Parcelable
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.lifecycle.lifecycleScope
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.launch
import kotlinx.parcelize.Parcelize
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.repository.auth.isLoggedIn
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import javax.inject.Inject

@AndroidEntryPoint
class ProductDetailsActivity : ComponentActivity() {

    companion object {
        const val PRODUCT_ID_EXTRA = "productIdExtra"
    }

    private val productId by lazy {
        val extra = if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.TIRAMISU)
            intent.getParcelableExtra(
                PRODUCT_ID_EXTRA,
                ProductIdExtra::class.java
            )
        else
            intent.getParcelableExtra(PRODUCT_ID_EXTRA)

        checkNotNull(extra) { "No product id extra found in intent" }
        extra.id
    }

    private val vm by viewModels<ProductDetailsScreenViewModel>()

    @Inject
    lateinit var authRepository: IAuthRepository

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        lifecycleScope.launch {
            vm.stateFlow.collect { state ->
                if (state is ProductDetailsScreenState.Idle) vm.fetchProductById(productId)
                if (state is ProductDetailsScreenState.LoadedProduct) {
                    vm.fetchProductDetails(productId, authRepository.authState.value.isLoggedIn())
                }
            }
        }

        setContent {
            MarkettrackerTheme {
                ProductDetailsScreen(onBackRequest = { finish() }, vm, authRepository)
            }
        }
    }
}

@Parcelize
data class ProductIdExtra(val id: String) : Parcelable
