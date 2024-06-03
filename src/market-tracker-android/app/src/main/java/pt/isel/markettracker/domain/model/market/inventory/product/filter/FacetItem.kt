package pt.isel.markettracker.domain.model.market.inventory.product.filter

data class FacetItem<T>(val id: T, val name: String, val count: Int, val isSelected: Boolean = false)

fun <T> List<FacetItem<T>>.isSelected(id: T) = any { it.id == id && it.isSelected }

fun <T> List<FacetItem<T>>.toggleSelection(id: T) =
    map { facetItem ->
        if (facetItem.id == id) {
            facetItem.copy(isSelected = !facetItem.isSelected)
        } else {
            facetItem
        }
    }.sortedByDescending { it.isSelected }

fun <T> List<FacetItem<T>>.resetSelections() =
    map { it.copy(isSelected = false) }