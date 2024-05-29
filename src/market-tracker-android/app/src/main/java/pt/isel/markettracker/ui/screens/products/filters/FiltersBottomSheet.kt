package pt.isel.markettracker.ui.screens.products.filters

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ModalBottomSheet
import androidx.compose.material3.rememberModalBottomSheetState
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsFilters
import pt.isel.markettracker.domain.model.market.inventory.product.filter.resetBrands
import pt.isel.markettracker.domain.model.market.inventory.product.filter.resetCategories
import pt.isel.markettracker.domain.model.market.inventory.product.filter.resetCompanies
import pt.isel.markettracker.domain.model.market.inventory.product.filter.toggleBrandSelection
import pt.isel.markettracker.domain.model.market.inventory.product.filter.toggleCategorySelection
import pt.isel.markettracker.domain.model.market.inventory.product.filter.toggleCompanySelection
import pt.isel.markettracker.ui.screens.products.filters.facets.CheckboxFacetRow
import pt.isel.markettracker.ui.screens.products.filters.facets.FacetRow
import pt.isel.markettracker.ui.screens.products.filters.facets.FacetSection

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun FiltersBottomSheet(
    isFiltersOpen: Boolean,
    onDismissRequest: () -> Unit,
    filters: ProductsFilters,
    onFiltersChange: (ProductsFilters) -> Unit,
    isLoading: Boolean
) {
    val sheetState = rememberModalBottomSheetState(skipPartiallyExpanded = true)

    if (isFiltersOpen) {
        ModalBottomSheet(
            modifier = Modifier.fillMaxHeight(0.7F),
            onDismissRequest = onDismissRequest,
            sheetState = sheetState,
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(horizontal = 16.dp)
                    .padding(bottom = 42.dp)
                    .verticalScroll(rememberScrollState()),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(12.dp, Alignment.Top)
            ) {
                FacetSection(
                    facets = filters.brands,
                    title = stringResource(id = R.string.brands_title),
                    onFacetsReset = {
                        onFiltersChange(filters.resetBrands())
                    }
                ) {
                    CheckboxFacetRow(
                        facet = it,
                        title = it.name,
                        enabled = !isLoading,
                        onClick = { brand ->
                            onFiltersChange(filters.toggleBrandSelection(brand))
                        }
                    )
                }

                FacetSection(
                    facets = filters.companies,
                    title = stringResource(id = R.string.companies_title),
                    onFacetsReset = {
                        onFiltersChange(filters.resetCompanies())
                    }
                ) {
                    CheckboxFacetRow(
                        facet = it,
                        title = it.name,
                        enabled = !isLoading,
                        onClick = { company ->
                            onFiltersChange(filters.toggleCompanySelection(company))
                        }
                    )
                }

                FacetSection(
                    facets = filters.categories,
                    title = stringResource(id = R.string.categories_title),
                    onFacetsReset = {
                        onFiltersChange(filters.resetCategories())
                    }
                ) {
                    FacetRow(
                        facet = it,
                        title = it.name,
                        enabled = !isLoading,
                        onClick = { category ->
                            onFiltersChange(filters.toggleCategorySelection(category))
                        }
                    )
                }
            }
        }
    }
}