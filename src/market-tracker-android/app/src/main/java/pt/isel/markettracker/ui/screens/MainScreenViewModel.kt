package pt.isel.markettracker.ui.screens

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import pt.isel.markettracker.navigation.Destination

class MainScreenViewModel : ViewModel() {
    companion object {
        fun factory() = viewModelFactory {
            initializer { MainScreenViewModel() }
        }
    }

    private val currentScreenFlow: MutableStateFlow<Destination> =
    MutableStateFlow(Destination.HOME)

    val currentScreen: Flow<Destination>
        get() = currentScreenFlow.asStateFlow()

    fun navigateTo(destination: Destination) {
        currentScreenFlow.value = destination
    }
}