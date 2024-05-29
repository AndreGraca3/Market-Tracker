package pt.isel.markettracker.domain.model.market.inventory.product.filter

import pt.isel.markettracker.domain.model.market.inventory.product.ProductsFacetsCounters
import pt.isel.markettracker.ui.screens.products.ProductsSortOption

data class ProductsQuery(
    val searchTerm: String? = null,
    val filters: ProductsFilters = ProductsFilters(),
    val sortOption: ProductsSortOption = ProductsSortOption.Popularity
)

fun ProductsQuery.replaceFacets(facets: ProductsFacetsCounters) =
    copy(filters = filters.replace(facets))

data class ProductsFilters(
    val brands: List<FacetItem<Int>> = emptyList(),
    val companies: List<FacetItem<Int>> = emptyList(),
    val categories: List<FacetItem<Int>> = emptyList(),
    val minRating: String? = null,
    val maxRating: String? = null
)

fun ProductsFilters.toggleBrandSelection(brandId: Int) =
    copy(brands = brands.toggleSelection(brandId).sortedByDescending { it.isSelected })

fun ProductsFilters.resetBrands() = copy(brands = brands.resetSelections())

fun ProductsFilters.toggleCompanySelection(companyId: Int) =
    copy(companies = companies.toggleSelection(companyId).sortedByDescending { it.isSelected })

fun ProductsFilters.resetCompanies() = copy(companies = companies.resetSelections())

fun ProductsFilters.toggleCategorySelection(categoryId: Int) =
    copy(categories = categories.toggleSelection(categoryId).sortedByDescending { it.isSelected })

fun ProductsFilters.resetCategories() = copy(categories = categories.resetSelections())

fun ProductsFilters.replace(facets: ProductsFacetsCounters) =
    copy(
        brands = facets.brands.map {
            FacetItem(
                it.id,
                it.name,
                it.count
            )
        },
        companies = facets.companies.map {
            FacetItem(
                it.id,
                it.name,
                it.count
            )
        },
        categories = facets.categories.map {
            FacetItem(
                it.id,
                it.name,
                it.count
            )
        }
    )