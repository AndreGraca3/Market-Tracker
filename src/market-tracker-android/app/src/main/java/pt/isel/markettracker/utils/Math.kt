package pt.isel.markettracker.utils

fun Int.centToEuro(): String {
    val euros = this.toDouble() / 100
    return "%.2f".format(euros)
}
