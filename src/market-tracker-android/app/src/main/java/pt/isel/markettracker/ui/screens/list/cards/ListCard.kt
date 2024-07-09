package pt.isel.markettracker.ui.screens.list.cards

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.Primary900
import pt.isel.markettracker.utils.advanceShadow

@Composable
fun ListCard(
    isLoading: Boolean,
    listNameContent: @Composable () -> Unit,
    listIconsContent: @Composable () -> Unit,
    loadingContent: @Composable () -> Unit,
    modifier: Modifier = Modifier,
) {
    val shape = RoundedCornerShape(8.dp)

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier
            .size(width = 350.dp, 125.dp)
    ) {
        Card(
            modifier = modifier
                .fillMaxSize()
                .clip(shape)
                .padding(2.dp)
                .border(2.dp, Color.Black.copy(.6F), shape)
                .advanceShadow(Primary900, blurRadius = 24.dp),
            colors = CardDefaults.cardColors(Color.White)
        ) {
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(14.dp, 8.dp),
                contentAlignment = Alignment.Center
            ) {
                if (isLoading) {
                    loadingContent()
                } else {
                    Row {
                        Column {
                            Box(
                                contentAlignment = Alignment.Center,
                                modifier = Modifier
                                    .fillMaxHeight()
                                    .fillMaxWidth(0.7F)
                            ) {
                                listNameContent()
                            }
                        }

                        Column {
                            Box(
                                modifier = Modifier
                                    .padding(
                                        horizontal = 20.dp
                                    )
                                    .fillMaxSize(),
                                contentAlignment = Alignment.Center
                            ) {
                                listIconsContent()
                            }
                        }
                    }
                }
            }
        }
    }
}
