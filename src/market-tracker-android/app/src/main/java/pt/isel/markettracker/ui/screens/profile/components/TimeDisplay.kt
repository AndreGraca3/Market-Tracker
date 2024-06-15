package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.text.style.TextAlign
import kotlinx.coroutines.delay
import pt.isel.markettracker.utils.timeSince
import java.time.LocalDateTime

@Composable
fun TimeDisplay(time: LocalDateTime) {
    val since = timeSince(time)
    val sinceParts = since.split(" ")
    val joinedValue = sinceParts.first().toInt()
    val unit = sinceParts.last()

    var actualTime by rememberSaveable { mutableIntStateOf(if (joinedValue - 50 < 0) 0 else joinedValue - 50) }

    LaunchedEffect(Unit) {
        while (true) {
            if (actualTime == joinedValue) break // this has to be done first to not keep counting when there is a recomposition
            delay(10L)
            actualTime += 1
        }
    }

    Text(
        text = "A poupar com o Market Tracker há $actualTime $unit",
        maxLines = 1,
        textAlign = TextAlign.Center,
    )
}