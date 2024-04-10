package pt.isel.markettracker.ui.screens.login

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory

class LoginScreenViewModel : ViewModel() {

    companion object {
        fun factory() = viewModelFactory {
            initializer { LoginScreenViewModel() }
        }
    }

    var email by mutableStateOf("")
    var password by mutableStateOf("")
}