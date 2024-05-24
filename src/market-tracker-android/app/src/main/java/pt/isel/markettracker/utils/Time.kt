package pt.isel.markettracker.utils

import java.time.Duration
import java.time.LocalDateTime
import java.util.Date

fun timeSince(date: LocalDateTime): String {
    val now = LocalDateTime.now()
    val duration = Duration.between(date, now)

    val seconds = duration.seconds
    val minutes = duration.toMinutes()
    val hours = duration.toHours()
    val days = duration.toDays()

    return when {
        seconds < 60 -> "$seconds segundos"
        minutes < 60 -> "$minutes minutos"
        hours < 24 -> "$hours horas"
        else -> "$days dias"
    }
}