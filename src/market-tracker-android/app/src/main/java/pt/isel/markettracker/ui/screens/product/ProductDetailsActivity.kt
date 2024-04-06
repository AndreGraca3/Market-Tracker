package pt.isel.markettracker.ui.screens.product

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

class ProductDetailsActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                ProductDetailsScreen()
            }
        }
    }
}