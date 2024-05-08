package pt.isel.markettracker.domain.model.market.inventory.product.filter

data class FacetItem<T>(val item: T, val count: Int, val isSelected: Boolean)

fun <T> List<FacetItem<T>>.isSelected(item: T) = any { it.item == item && it.isSelected }

fun <T> List<FacetItem<T>>.toggleSelection(item: T) =
    map { facetItem ->
        if (facetItem.item == item) {
            facetItem.copy(isSelected = !facetItem.isSelected)
        } else {
            facetItem
        }
    }.sortedByDescending { it.isSelected }

fun <T> List<FacetItem<T>>.resetSelections() =
    map { it.copy(isSelected = false) }