package pt.isel.markettracker

import android.app.Application
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.preferencesDataStore
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import okhttp3.OkHttpClient
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.operations.user.UserService
import java.util.concurrent.TimeUnit

/**
 * The application's class used to resolve dependencies, acting as a Service Locator.
 * Dependencies are then injected manually by each Android Component (e.g Activity, Service, etc.).
 */
class MarketTrackerApplication : Application(), MarketTrackerDependencyProvider {

    companion object {
        /**
         * The timeout for HTTP requests
         */
        private const val timeout = 10L
        const val MT_DATASTORE = "market_tracker_datastore"
    }

    private val dataStore: DataStore<Preferences> by preferencesDataStore(MT_DATASTORE)

    /*
    override val preferencesRepository: PreferencesDataStore
        get() = PreferencesDataStore(dataStore)
    */

    /**
     * The HTTP client used to perform HTTP requests
     */
    override val httpClient: OkHttpClient =
        OkHttpClient.Builder()
            .callTimeout(timeout, TimeUnit.SECONDS)
            .build()

    /**
     * The JSON serializer/deserializer used to convert JSON into Models
     */
    override val gson: Gson = GsonBuilder().create()

    override val userService: IUserService
        get() = UserService(httpClient, gson)
}