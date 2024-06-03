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
        seconds < 60 -> "$seconds ${if (seconds == 1L) "segundo" else "segundos"}"
        minutes < 60 -> "$minutes ${if (minutes == 1L) "minuto" else "minutos"}"
        hours < 24 -> "$hours ${if (hours == 1L) "hora" else "horas"}"
        else -> "$days ${if (days == 1L) "dia" else "dias"}"
    }
}