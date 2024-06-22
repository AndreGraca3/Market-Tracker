package pt.isel.markettracker

import android.app.Application
import android.content.Context
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.preferencesDataStore
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import com.google.gson.JsonDeserializer
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.components.SingletonComponent
import kotlinx.coroutines.runBlocking
import okhttp3.Cookie
import okhttp3.CookieJar
import okhttp3.HttpUrl
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.market.inventory.product.ProductUnit
import pt.isel.markettracker.http.service.operations.alert.AlertService
import pt.isel.markettracker.http.service.operations.alert.IAlertService
import pt.isel.markettracker.http.service.operations.auth.AuthService
import pt.isel.markettracker.http.service.operations.auth.IAuthService
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.operations.list.ListService
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.http.service.operations.product.ProductService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.operations.user.UserService
import pt.isel.markettracker.repository.auth.AuthRepository
import pt.isel.markettracker.repository.auth.IAuthRepository
import java.time.Instant
import java.time.LocalDateTime
import java.time.ZoneOffset
import java.util.concurrent.TimeUnit
import javax.inject.Singleton

@Module
@InstallIn(SingletonComponent::class)
class AppModule {
    private val Context.dataStore: DataStore<Preferences> by preferencesDataStore(
        name = MarketTrackerApplication.MT_DATASTORE
    )

    @Provides
    @Singleton
    fun provideDataStore(appContext: Application): DataStore<Preferences> {
        return appContext.dataStore
    }

    @Provides
    @Singleton
    fun provideAuthRepository(dataStore: DataStore<Preferences>): IAuthRepository {
        return AuthRepository(dataStore)
    }

    @Provides
    @Singleton
    fun provideHttpClient(authRepository: IAuthRepository): OkHttpClient {
        return OkHttpClient.Builder()
            .connectTimeout(10, TimeUnit.SECONDS)
            .callTimeout(10, TimeUnit.SECONDS)
            .cookieJar(object : CookieJar {
                override fun saveFromResponse(url: HttpUrl, cookies: List<Cookie>) {
                    val responseToken = cookies.find { it.name == "Authorization" }?.value
                    runBlocking {
                        authRepository.setToken(responseToken)
                    }
                }

                override fun loadForRequest(url: HttpUrl): List<Cookie> {
                    val token = runBlocking { authRepository.getToken() }
                    return if (token != null) {
                        listOf(
                            Cookie.Builder()
                                .name("Authorization")
                                .value(token)
                                .domain("markettracker.com")
                                .build()
                        )
                    } else {
                        emptyList()
                    }
                }
            })
            .build()
    }

    @Provides
    @Singleton
    fun provideGson(): Gson {
        return GsonBuilder()
            .serializeNulls()
            .registerTypeAdapter(
                LocalDateTime::class.java,
                JsonDeserializer { json, _, _ ->
                    LocalDateTime.ofInstant(
                        Instant.parse(json.asString),
                        ZoneOffset.UTC
                    )
                })
            .registerTypeAdapter(
                ProductUnit::class.java,
                JsonDeserializer { json, _, _ ->
                    ProductUnit.fromTitle(json.asString)
                })
            .create()
    }

    @Provides
    @Singleton
    fun provideListService(httpClient: OkHttpClient, gson: Gson): IListService {
        return ListService(httpClient, gson)
    }

    @Provides
    @Singleton
    fun provideProductService(httpClient: OkHttpClient, gson: Gson): IProductService {
        return ProductService(httpClient, gson)
    }

    @Provides
    @Singleton
    fun provideUserService(httpClient: OkHttpClient, gson: Gson): IUserService {
        return UserService(httpClient, gson)
    }

    @Provides
    @Singleton
    fun provideTokenService(httpClient: OkHttpClient, gson: Gson): IAuthService {
        return AuthService(httpClient, gson)
    }

    @Provides
    @Singleton
    fun provideAlertService(httpClient: OkHttpClient, gson: Gson): IAlertService {
        return AlertService(httpClient, gson)
    }
}