package pt.isel.markettracker.domain.model.market.inventory.product

enum class ProductUnit(val title: String, val baseUnit: String) {
    KILOGRAMS("kilogramas", "Kg"),
    GRAMS("gramas", "g"),
    LITERS("litros", "L"),
    MILLILITERS("mililitros", "mL"),
    UNIT("unidades", "uni");

    companion object {
        fun fromBaseUnit(baseUnit: String): ProductUnit {
            return entries.find { it.baseUnit == baseUnit } ?: UNIT
        }

        fun fromTitle(title: String): ProductUnit {
            return entries.find { it.title == title } ?: UNIT
        }
    }
}