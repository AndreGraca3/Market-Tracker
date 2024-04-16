package pt.isel.markettracker

import android.app.Application
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.preferencesDataStore
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import dagger.hilt.android.HiltAndroidApp
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.operations.user.UserService
import java.util.concurrent.TimeUnit

@HiltAndroidApp
class MarketTrackerApplication : Application() {
    companion object {
        /**
         * The timeout for HTTP requests
         */
        private const val timeout = 10L

        /**
         * The DataStore name
         */
        const val MT_DATASTORE = "pt.isel.market_tracker.datastore"
    }
}