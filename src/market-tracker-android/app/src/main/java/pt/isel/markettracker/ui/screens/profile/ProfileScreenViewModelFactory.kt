package pt.isel.markettracker.ui.screens.profile

import android.content.ContentResolver
import dagger.assisted.AssistedFactory

@AssistedFactory
interface ProfileScreenViewModelFactory {
    fun create(contentResolver: ContentResolver): ProfileScreenViewModel
}