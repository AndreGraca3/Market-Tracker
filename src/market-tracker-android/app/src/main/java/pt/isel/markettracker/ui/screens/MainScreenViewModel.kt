package pt.isel.markettracker.ui.screens

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import kotlinx.coroutines.ExperimentalCoroutinesApi
import kotlinx.coroutines.async
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking
import pt.isel.markettracker.domain.infrastruture.PreferencesDataStore
import pt.isel.markettracker.domain.infrastruture.PreferencesRepository
import pt.isel.markettracker.navigation.Destination

class MainScreenViewModel(
    private val preferences: PreferencesRepository
) : ViewModel() {
    companion object {
        fun factory(preferences: PreferencesRepository) = viewModelFactory {
            initializer { MainScreenViewModel(preferences) }
        }
    }

    private val currentScreenFlow: MutableStateFlow<Destination> =
    MutableStateFlow(Destination.HOME)

    val currentScreen: Flow<Destination>
        get() = currentScreenFlow.asStateFlow()

    fun navigateTo(destination: Destination) {
        currentScreenFlow.value = destination
    }

    /**
     * I don't understand how and why this works XD
     */
    @OptIn(ExperimentalCoroutinesApi::class)
    fun isLoggedIn(): Boolean {
        runBlocking {
            preferences.getToken()
        }
        return viewModelScope.async {
            preferences.getToken()
        }.getCompleted() != null
    }
}