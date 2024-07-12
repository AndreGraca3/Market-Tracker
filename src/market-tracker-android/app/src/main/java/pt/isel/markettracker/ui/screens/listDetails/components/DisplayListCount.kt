package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.mainFont
import pt.isel.markettracker.utils.centToEuro

@Composable
fun DisplayListCount(
    totalPrice: Int,
    amountOfProducts: Int,
) {
    val borderWidth = 2.dp
    val borderColor = Color.Black

    Box(
        modifier = Modifier
            .fillMaxWidth()
            .fillMaxHeight(0.05F)
            .border(width = borderWidth, color = borderColor),
    ) {
        Row(
            horizontalArrangement = Arrangement.Center,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Box(
                modifier = Modifier
                    .fillMaxHeight()
                    .fillMaxWidth(0.5F)
                    .border(width = borderWidth, color = borderColor)
                    .padding(horizontal = 5.dp),
                contentAlignment = Alignment.CenterStart
            ) {
                Text(
                    text = "Produtos: $amountOfProducts",
                    fontFamily = mainFont,
                )
            }

            Box(
                modifier = Modifier
                    .fillMaxHeight()
                    .fillMaxWidth()
                    .padding(horizontal = 5.dp),
                contentAlignment = Alignment.CenterStart
            ) {
                Text(
                    text = "total:  ${totalPrice.centToEuro()} €",
                    fontFamily = mainFont,
                )
            }
        }
    }
}