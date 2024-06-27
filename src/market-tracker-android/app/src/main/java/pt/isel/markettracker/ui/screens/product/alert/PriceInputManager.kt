package pt.isel.markettracker.ui.screens.product.alert

data class PriceInputManager(
    val maxPrice: Double,
    val priceText: String,
    val isValid: Boolean
) {
    val isInsideBounds: Boolean
        get() = isInBounds(getPriceValue())

    private fun isInBounds(price: Double) = price in 0.0..maxPrice

    fun getPriceValue(): Double {
        return priceText.replace(",", ".").toDoubleOrNull() ?: 0.0
    }

    fun setNewPrice(newPriceText: String): PriceInputManager {
        val formattedPriceText = newPriceText.replace(".", ",")

        if (formattedPriceText.count { it == ',' } > 1) {
            return this
        }

        val normalizedPriceText =
            if (formattedPriceText == "." || formattedPriceText == ",") "0," else formattedPriceText

        val maxIntLength = maxPrice.toInt().toString().length + 1
        if (normalizedPriceText.contains(",")) {
            val numericParts = normalizedPriceText.split(",")
            if (numericParts[0].length > maxIntLength || numericParts[1].length > 2) return this
        } else {
            if (normalizedPriceText.length > maxIntLength
            ) return this
        }

        val priceVal = normalizedPriceText.replace(",", ".").toDoubleOrNull()

        return if (priceVal == null) {
            copy(priceText = normalizedPriceText, isValid = false)
        } else {
            if (isInBounds(priceVal)) {
                copy(priceText = normalizedPriceText, isValid = true)
            } else {
                copy(priceText = normalizedPriceText, isValid = false)
            }
        }
    }
}
