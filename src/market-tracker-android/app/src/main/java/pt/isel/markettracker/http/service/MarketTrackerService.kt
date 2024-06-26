package pt.isel.markettracker.http.service

import android.util.Log
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import kotlinx.coroutines.suspendCancellableCoroutine
import okhttp3.Call
import okhttp3.Callback
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import okhttp3.Response
import okio.IOException
import pt.isel.markettracker.BuildConfig
import pt.isel.markettracker.http.problem.InternalServerErrorProblem
import pt.isel.markettracker.http.problem.Problem
import pt.isel.markettracker.http.service.result.APIException
import java.lang.reflect.Type
import java.net.URL
import kotlin.coroutines.resumeWithException

abstract class MarketTrackerService {
    companion object {
        const val MT_API_URL = BuildConfig.API_URL
    }

    abstract val httpClient: OkHttpClient
    abstract val gson: Gson

    protected suspend inline fun <reified T> requestHandler(
        path: String,
        method: HttpMethod,
        body: Any? = null
    ): T {
        val url = URL(MT_API_URL + path)
        Log.v("requestHandler", "Request to API: $url")
        val request = Request.Builder().buildRequest(
            url,
            method,
            body
        )

        return suspendCancellableCoroutine {
            val call = httpClient.newCall(request)
            call.enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    it.resumeWithException(e)
                }

                override fun onResponse(call: Call, response: Response) {
                    val responseBody = response.body?.string()
                    if (!response.isSuccessful) {
                        Log.v("requestHandler", "Content type: ${response.body?.contentType()}")
                        if (response.code < 500 && response.body?.contentType() == Problem.MEDIA_TYPE) {
                            Log.v("requestHandler", "Response body: $responseBody")
                            val problem = gson.fromJson(responseBody, Problem::class.java)
                            Log.v("requestHandler", "Result of call to API: $problem")
                            it.resumeWithException(APIException(problem))
                        } else {
                            it.resumeWithException(APIException(InternalServerErrorProblem()))
                            return
                        }
                    } else {
                        val type: Type = object : TypeToken<T>() {}.type
                        val res: T = gson.fromJson(responseBody, type)
                        it.resumeWith(Result.success(res))
                    }
                }
            })
            it.invokeOnCancellation { call.cancel() }
        }
    }

    protected fun Request.Builder.buildRequest(
        url: URL,
        method: HttpMethod,
        body: Any? = null
    ): Request {
        val hypermediaType = "application/json".toMediaType()
        val request = this
            .url(url)
            .addHeader("accept", hypermediaType.toString())

        when (method) {
            HttpMethod.GET -> request.get()
            HttpMethod.POST -> request.post(gson.toJson(body).toRequestBody(hypermediaType))
            HttpMethod.PUT -> request.put(gson.toJson(body).toRequestBody(hypermediaType))
            HttpMethod.PATCH -> request.patch(gson.toJson(body).toRequestBody(hypermediaType))
            HttpMethod.DELETE -> request.delete()
        }

        return request.build()
    }

    protected enum class HttpMethod {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE,
    }
}

fun parameterizedURL(schema: URL, vararg pathVariables: Any): URL {
    val path = schema.path.split("/")
    val mutablePathVariable = pathVariables.toMutableList()
    val parameterizedPath = path.map {
        if (it.contains(":")) {
            mutablePathVariable.removeFirst().toString()
        } else "/$it"
    }.joinToString("") { it }
    return URL(
        "${schema.protocol}://${schema.authority}$parameterizedPath"
    )
}