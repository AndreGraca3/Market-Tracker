package pt.isel.markettracker

import android.app.Application
import dagger.hilt.android.HiltAndroidApp

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