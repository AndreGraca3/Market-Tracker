package pt.isel.markettracker.ui.screens.profile

import androidx.lifecycle.ViewModel
import dagger.hilt.android.lifecycle.HiltViewModel
import pt.isel.markettracker.domain.user.User
import javax.inject.Inject

@HiltViewModel
class ProfileScreenViewModel @Inject constructor(
) : ViewModel() {
    val user = User(
        "Digo",
        "Diogo",
        "Diogo@gmail.com",
        "123",
        "avatar_url"
    )
}