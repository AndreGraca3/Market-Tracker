package pt.isel.markettracker.ui.screens.profile

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import pt.isel.markettracker.domain.user.User

class ProfileScreenViewModel : ViewModel() {

    companion object {
        fun factory() =
            viewModelFactory {
                initializer { ProfileScreenViewModel() }
            }
    }

    val user = User(
        "Digo",
        "Diogo",
        "Diogo@gmail.com",
        "123",
        "avatar_url"
    )
}