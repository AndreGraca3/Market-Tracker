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
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.market.inventory.ProductUnit
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.http.service.operations.product.ProductService
import pt.isel.markettracker.http.service.operations.token.ITokenService
import pt.isel.markettracker.http.service.operations.token.TokenService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.operations.user.UserService
import pt.isel.markettracker.repository.auth.AuthRepository
import pt.isel.markettracker.repository.auth.IAuthRepository
import java.time.LocalDateTime
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

    @Singleton
    @Provides
    fun provideAuthRepository(): IAuthRepository {
        return AuthRepository()
    }

    @Provides
    @Singleton
    fun provideHttpClient(): OkHttpClient {
        return OkHttpClient.Builder()
            .callTimeout(10, TimeUnit.SECONDS)
            .build()
    }

    @Provides
    @Singleton
    fun provideGson(): Gson {
        return GsonBuilder()
            .registerTypeAdapter(LocalDateTime::class.java, JsonDeserializer { json, _, _ ->
                LocalDateTime.parse(json.asString)
            })
            .registerTypeAdapter(ProductUnit::class.java, JsonDeserializer { json, _, _ ->
                ProductUnit.fromTitle(json.asString)
            })
            .create()
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
    fun provideTokenService(httpClient: OkHttpClient, gson: Gson): ITokenService {
        return TokenService(httpClient, gson)
    }
}