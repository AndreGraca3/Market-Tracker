package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.width
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun InCompanyHeader(companyLogoUrl: String) {
    Row(
        modifier = Modifier.height(18.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Text(
            text = "No(a)",
            style = MarketTrackerTypography.bodyMedium
        )
        Spacer(modifier = Modifier.width(8.dp))
        SubcomposeAsyncImage(
            model = companyLogoUrl,
            contentDescription = "company logo",
            contentScale = ContentScale.Fit,
            loading = {
                CircularProgressIndicator(
                    modifier = Modifier
                        .height(16.dp)
                        .width(16.dp),
                    strokeWidth = 2.dp
                )
            }
        )
    }
}
