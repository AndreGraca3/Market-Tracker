package pt.isel.markettracker.ui.screens.product.prices

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.wrapContentHeight
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.market.Company
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.StoreOffer
import pt.isel.markettracker.dummy.dummyCompanyPrices
import pt.isel.markettracker.ui.screens.product.stores.StoresBottomSheet
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun CompanyRow(
    companyPrices: CompanyPrices,
    showOptions: Boolean,
    onPriceSectionClick: (Int, Int) -> Unit,
    onAddToListClick: (StoreOffer) -> Unit,
) {
    var showCompanyStores by rememberSaveable { mutableStateOf(false) }

    var selectedStoreId by rememberSaveable { mutableIntStateOf(companyPrices.stores.first().store.id) }
    val selectedStoreOffer = companyPrices.stores.first { it.store.id == selectedStoreId }

    Row(
        modifier = Modifier
            .fillMaxWidth()
            .wrapContentHeight()
            .clickable {
                onPriceSectionClick(
                    selectedStoreOffer.store.id,
                    selectedStoreOffer.price.finalPrice
                )
            },
        horizontalArrangement = Arrangement.spacedBy(14.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Column(
            modifier = Modifier.weight(1F),
            verticalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            Box(
                modifier = Modifier
                    .fillMaxSize(0.5F),
                contentAlignment = Alignment.CenterStart
            ) {
                SubcomposeAsyncImage(
                    model = companyPrices.logoUrl,
                    loading = {
                        CircularProgressIndicator()
                    },
                    contentDescription = "Company Logo",
                    contentScale = ContentScale.Fit
                )
            }

            Text(
                text = selectedStoreOffer.store.name,
                modifier = Modifier
                    .clip(RoundedCornerShape(6.dp))
                    .clickable {
                        showCompanyStores = true
                    }
                    .padding(vertical = 4.dp),
                style = MarketTrackerTypography.bodySmall,
                fontWeight = FontWeight.SemiBold,
                color = Color.Gray,
                maxLines = 1,
                overflow = TextOverflow.Ellipsis
            )
        }

        if (selectedStoreOffer.isAvailable) {
            CompanyPriceBox(
                price = selectedStoreOffer.price,
                lastChecked = selectedStoreOffer.lastChecked,
                showOptions = showOptions,
                onAddToListClick = {
                    onAddToListClick(
                        selectedStoreOffer.toStoreOffer(
                            Company(
                                companyPrices.id,
                                companyPrices.name,
                                companyPrices.logoUrl
                            )
                        )
                    )
                }
            )
        } else {
            Text(
                text = stringResource(id = R.string.not_available),
                style = MarketTrackerTypography.bodySmall,
                color = Color.Red
            )
        }
    }

    StoresBottomSheet(
        showStores = showCompanyStores,
        storesPrices = companyPrices.stores,
        onStoreSelect = { selectedStoreId = it },
        onDismissRequest = { showCompanyStores = false }
    )

}

@Preview
@Composable
fun PriceRowPreview() {
    CompanyRow(dummyCompanyPrices.first(), true, { _, _ -> }, { })
}