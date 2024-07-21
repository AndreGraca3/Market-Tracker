package pt.isel.markettracker.ui.screens.priceHistory

import android.util.Log
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AddAlert
import androidx.compose.material.icons.filled.Remove
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.patrykandpatrick.vico.core.cartesian.data.CartesianChartModelProducer
import com.patrykandpatrick.vico.core.cartesian.data.lineSeries
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPriceHistoryEntry
import pt.isel.markettracker.ui.components.buttons.MarketTrackerBackButton
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.priceHistory.chart.MarketTrackerLineChart
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.theme.mainFont
import pt.isel.markettracker.utils.centToEuro

@Composable
fun PriceHistoryScreenView(
    state: PriceHistoryScreenState,
    hasAlert: Boolean,
    onAlertClick: () -> Unit,
    onBackRequested: () -> Unit,
) {
    Scaffold(
        topBar =
        {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Red)
                    .padding(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Box(
                    modifier = Modifier.fillMaxWidth()
                ) {
                    HeaderLogo(
                        modifier = Modifier
                            .align(alignment = Alignment.CenterStart)
                            .size(48.dp)
                    )
                    Text(
                        text = "Histórico 📈",
                        color = Color.White,
                        fontFamily = mainFont,
                        fontSize = 30.sp,
                        modifier = Modifier
                            .align(alignment = Alignment.Center)
                    )

                    Box(
                        modifier = Modifier.align(alignment = Alignment.CenterEnd)
                    ) {
                        MarketTrackerBackButton(onBackRequested = onBackRequested)
                    }
                }
            }
        },
    ) { paddingValues ->
        Box(
            modifier = Modifier
                .fillMaxWidth()
                .padding(paddingValues),
            contentAlignment = Alignment.Center
        ) {
            when (state) {
                is PriceHistoryScreenState.Loaded -> {
                    val prices = state.priceHistory.history
                    val filledPrices = if (prices.size >= 15) {
                        prices.takeLast(15)
                    } else {
                        val last = prices.last()
                        val newList =
                            prices.map { it.copy(date = it.date.plusDays(1L)) }.toMutableList()
                        Log.v("last", "$last")
                        for (i in prices.size until 15) {
                            newList.add(
                                ProductPriceHistoryEntry(
                                    price = last.price,
                                    date = last.date.plusDays(i + 1L),
                                )
                            )
                        }
                        Log.v("NewList", "$newList")
                        newList
                    }

                    val modelProducer = remember { CartesianChartModelProducer() }
                    LaunchedEffect(Unit) {
                        withContext(Dispatchers.Default) {
                            modelProducer.runTransaction {
                                lineSeries {
                                    series(
                                        x = filledPrices.map { it.date.dayOfMonth },
                                        filledPrices.map { (it.price.toDouble() / 100) })
                                }
                            }
                        }
                    }

                    Column(
                        modifier = Modifier.padding(16.dp),
                    ) {
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.SpaceBetween
                        ) {
                            Button(onClick = onAlertClick) {
                                Row(
                                    verticalAlignment = Alignment.CenterVertically
                                ) {
                                    Icon(
                                        imageVector = if (hasAlert) Icons.Filled.Remove else Icons.Filled.AddAlert,
                                        contentDescription = "Alert"
                                    )

                                    Text(
                                        text = if (hasAlert) stringResource(id = R.string.remove_alert)
                                        else stringResource(id = R.string.set_alert)
                                    )
                                }
                            }
                        }

                        Box {
                            MarketTrackerLineChart(modelProducer)
                        }

                        Row {
                            Column {
                                Column {
                                    Text("Valor médio")
                                    Text("${(filledPrices.sumOf { it.price } / filledPrices.size).centToEuro()} €"
                                    )
                                }
                                Column {
                                    Text("Valor Máximo")
                                    Text(
                                        "${filledPrices.maxOfOrNull { it.price }?.centToEuro()} €"
                                    )
                                }
                            }
                        }
                        Row {
                            Column {
                                Column {
                                    Text("Nº de Listas")
                                    Text("${state.priceHistory.numberOfListPresent}")
                                }
                                Column {
                                    Text("Valor mínimo")
                                    Text(
                                        "${filledPrices.minOfOrNull { it.price }?.centToEuro()} €"
                                    )
                                }
                            }
                        }
                    }
                }

                is PriceHistoryScreenState.Loading -> {
                    LoadingIcon(text = "Carregando o Histórico de preços...")
                }

                else -> {
                    Column(
                        horizontalAlignment = Alignment.CenterHorizontally,
                    ) {
                        Image(
                            painter = painterResource(id = R.drawable.server_error),
                            contentDescription = "No products found"
                        )
                        Text(
                            text = stringResource(id = R.string.loading_error)
                        )
                    }
                }
            }
        }
    }
}