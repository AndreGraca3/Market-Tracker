package pt.isel.markettracker.domain.model.market.inventory.product.filter

import pt.isel.markettracker.domain.model.market.Company
import pt.isel.markettracker.domain.model.market.inventory.Brand
import pt.isel.markettracker.domain.model.market.inventory.Category
import pt.isel.markettracker.domain.model.market.inventory.product.ProductsFacetsCounters
import pt.isel.markettracker.ui.screens.ProductsSortOption

data class ProductsQuery(
    val searchTerm: String? = null,
    val filters: ProductsFilters = ProductsFilters(),
    val sortOption: ProductsSortOption = ProductsSortOption.Popularity
)

data class ProductsFilters(
    val brands: List<FacetItem<Brand>> = emptyList(),
    val companies: List<FacetItem<Company>> = emptyList(),
    val categories: List<FacetItem<Category>> = emptyList(),
    val minRating: String? = null,
    val maxRating: String? = null,
    val minPrice: Int = 0,
    val maxPrice: Int = Int.MAX_VALUE
)

fun ProductsFilters.toggleBrandSelection(brand: Brand) =
    copy(brands = brands.toggleSelection(brand))

fun ProductsFilters.resetBrands() = copy(brands = brands.resetSelections())

fun ProductsFilters.toggleCompanySelection(company: Company) =
    copy(companies = companies.toggleSelection(company))

fun ProductsFilters.resetCompanies() = copy(companies = companies.resetSelections())

fun ProductsFilters.toggleCategorySelection(category: Category) =
    copy(categories = categories.toggleSelection(category))

fun ProductsFilters.resetCategories() = copy(categories = categories.resetSelections())

fun ProductsFilters.replaceWithState(facets: ProductsFacetsCounters) =
    copy(
        brands = facets.brands.map {
            FacetItem(
                it.item,
                it.count,
                brands.isSelected(it.item)
            )
        }.sortedByDescending { it.isSelected },
        companies = facets.companies.map {
            FacetItem(
                it.item,
                it.count,
                companies.isSelected(it.item)
            )
        }.sortedByDescending { it.isSelected },
        categories = facets.categories.map {
            FacetItem(
                it.item,
                it.count,
                categories.isSelected(it.item)
            )
        }.sortedByDescending { it.isSelected }
    )