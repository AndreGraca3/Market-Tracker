package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.width
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import kotlinx.coroutines.delay
import pt.isel.markettracker.utils.timeSince
import java.time.LocalDateTime

@Composable
fun TimeDisplay(time: LocalDateTime) {
    val since = timeSince(time)
    val sinceParts = since.split(" ")
    val joinedValue = sinceParts.first().toInt()
    val unit = sinceParts.last()

    var actualTime by rememberSaveable {
        mutableIntStateOf(if (joinedValue - 50 < 0) 0 else joinedValue - 50)
    }

    LaunchedEffect(Unit) {
        while (true) {
            if (actualTime == joinedValue) break // this has to be done first to not keep counting when there is a recomposition
            delay(10L)
            actualTime += 1
        }
    }

    Row(
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.Center
    ) {

        val maxLines = 1
        val textAlign = TextAlign.Center
        val fontSize = 15.sp

        Text(
            text = "A poupar com o Market Tracker há",
            maxLines = maxLines,
            textAlign = textAlign,
            fontSize = fontSize
        )

        Text(
            text = "$actualTime",
            maxLines = maxLines,
            textAlign = textAlign,
            fontSize = fontSize,
            modifier = Modifier.width(
                width = getSpacing(joinedValue),
            )
        )

        Text(
            text = unit,
            maxLines = maxLines,
            textAlign = textAlign,
            fontSize = fontSize
        )
    }
}

private fun getSpacing(value: Int): Dp {
    return ("$value".length.times(10) + 5).dp
}