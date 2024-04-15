package pt.isel.markettracker.ui.screens.product.rating

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.BarChart
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material.icons.filled.ShoppingBasket
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.unit.dp

@Composable
fun ProductMetricsBox(favourites: Int, ratings: Int, lists: Int) {
    Row(
        horizontalArrangement = Arrangement.spacedBy(12.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        ProductMetric(imageVector = Icons.Default.Favorite, metric = favourites)

        ProductMetric(imageVector = Icons.Default.BarChart, metric = ratings)

        ProductMetric(imageVector = Icons.Default.ShoppingBasket, metric = lists)
    }

}

@Composable
fun ProductMetric(imageVector: ImageVector, metric: Int) {
    Column(
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Icon(imageVector = imageVector, contentDescription = null)
        Text(text = "$metric")
    }
}