package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import kotlinx.coroutines.delay
import pt.isel.markettracker.utils.timeSince
import java.time.LocalDateTime

@Composable
fun TimeDisplay(time: LocalDateTime) {
    val since = timeSince(time)
    val sinceParts = since.split(" ")
    var actualTime by rememberSaveable { mutableIntStateOf(0) }
    val joinedValue = sinceParts[0].toInt()

    LaunchedEffect(Unit) {
        while (true) {
            val delay = if (joinedValue - actualTime < 50) 10L else 5L
            delay(delay)
            actualTime += 1
            if (actualTime == joinedValue) break
        }
    }

    Text(text = "A poupar com o Market Tracker há $actualTime ${sinceParts[1]}")
}