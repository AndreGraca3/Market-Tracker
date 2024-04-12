package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier

@Composable
fun CompanyPrice(price: Int, logoUrl: String) {
    Column(
        modifier = Modifier
            .fillMaxWidth(),
        horizontalAlignment = Alignment.End,
    ) {
        ProductPrice(price)
        CompanyHeader(logoUrl)
    }
}