package pt.isel.markettracker.http.service.result

import android.util.Log
import pt.isel.markettracker.http.problem.InternalServerErrorProblem

sealed class APIResult<T>() {
    companion object {
        fun <T> success(value: T) = Success(value)
        fun <T> failure(exception: APIException) = Failure<T>(exception)
    }

    fun getOrNull(): T? = when (this) {
        is Success -> value
        else -> null
    }

    fun exceptionOrNull(): APIException? = when (this) {
        is Failure -> exception
        else -> null
    }

    fun getOrThrow(): T = when (this) {
        is Success -> value
        is Failure -> throw exception
    }

    fun onSuccess(block: (T) -> Unit): APIResult<T> {
        if (this is Success) block(value)
        return this
    }

    fun onFailure(block: (APIException) -> Unit): APIResult<T> {
        if (this is Failure) block(exception)
        return this
    }

    val isSuccess: Boolean
        get() = this is Success

    val isFailure: Boolean
        get() = this is Failure
}

data class Success<T>(val value: T) : APIResult<T>()
data class Failure<T>(val exception: APIException) : APIResult<T>()

inline fun <T> runCatchingAPIFailure(block: () -> T): APIResult<T> {
    return try {
        APIResult.success(block())
    } catch (e: Throwable) {
        Log.e("APIResult", "runCatchingAPIFailure", e)
        if (e is APIException) return APIResult.failure(e)
        APIResult.failure(APIException(InternalServerErrorProblem()))
    }
}