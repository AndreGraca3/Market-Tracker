package pt.isel.markettracker.utils

import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale

fun timeSince(timestamp: String): String {
    val format = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSSSS", Locale.getDefault())
    val date = format.parse(timestamp)
    val now = Date()

    val diff = now.time - date.time
    val seconds = diff / 1000
    val minutes = seconds / 60
    val hours = minutes / 60
    val days = hours / 24

    return when {
        days > 0 -> "$days dias"
        hours > 0 -> "$hours horas"
        minutes > 0 -> "$minutes minutos"
        else -> "$seconds segundos"
    }
}