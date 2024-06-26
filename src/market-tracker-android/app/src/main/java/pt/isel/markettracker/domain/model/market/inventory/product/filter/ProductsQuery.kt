package pt.isel.markettracker.domain.model.market.inventory.product.filter

import pt.isel.markettracker.domain.model.market.inventory.product.ProductsFacetsCounters

data class ProductsQuery(
    val searchTerm: String? = null,
    val filters: ProductsFilters = ProductsFilters(),
    val sortOption: ProductsSortOption
) {
    val hasFiltersApplied: Boolean
        get() = filters.brands.any { it.isSelected } ||
                filters.companies.any { it.isSelected } ||
                filters.categories.any { it.isSelected } ||
                filters.minRating != null ||
                filters.maxRating != null
}

fun ProductsQuery.replaceFacets(facets: ProductsFacetsCounters) =
    copy(filters = filters.replaceWithState(facets))

data class ProductsFilters(
    val brands: List<FacetItem<Int>> = emptyList(),
    val companies: List<FacetItem<Int>> = emptyList(),
    val categories: List<FacetItem<Int>> = emptyList(),
    val minRating: String? = null,
    val maxRating: String? = null
)

fun ProductsFilters.toggleBrandSelection(brandId: Int) =
    copy(brands = brands.toggleSelection(brandId))

fun ProductsFilters.resetAll() = copy(
    brands = brands.resetSelections(),
    companies = companies.resetSelections(),
    categories = categories.resetSelections(),
    minRating = null,
    maxRating = null
)

fun ProductsFilters.resetBrands() = copy(brands = brands.resetSelections())

fun ProductsFilters.toggleCompanySelection(companyId: Int) =
    copy(companies = companies.toggleSelection(companyId))

fun ProductsFilters.resetCompanies() = copy(companies = companies.resetSelections())

fun ProductsFilters.toggleCategorySelection(categoryId: Int) =
    copy(categories = categories.toggleSelection(categoryId))

fun ProductsFilters.resetCategories() = copy(categories = categories.resetSelections())

fun ProductsFilters.replaceWithState(facets: ProductsFacetsCounters) =
    copy(
        brands = facets.brands.map {
            FacetItem(
                it.id,
                it.name,
                it.count,
                brands.isSelected(it.id)
            )
        }.sortedByDescending { it.isSelected },
        companies = facets.companies.map {
            FacetItem(
                it.id,
                it.name,
                it.count,
                companies.isSelected(it.id)
            )
        }.sortedByDescending { it.isSelected },
        categories = facets.categories.map {
            FacetItem(
                it.id,
                it.name,
                it.count,
                categories.isSelected(it.id)
            )
        }.sortedByDescending { it.isSelected }
    )

enum class ProductsSortOption(val title: String) {
    Relevance("Relevância"),
    Popularity("Popularidade"),
    NameLowToHigh("Nome (A-Z)"),
    NameHighToLow("Nome (Z-A)"),
    RatingLowToHigh("Menor Avaliação"),
    RatingHighToLow("Maior Avaliação"),
    PriceLowToHigh("Menor Preço"),
    PriceHighToLow("Maior Preço");

    companion object {
        fun fromTitle(title: String) = entries.first { it.title == title }
    }
}