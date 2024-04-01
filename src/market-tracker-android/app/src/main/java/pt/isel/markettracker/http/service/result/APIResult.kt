package pt.isel.markettracker.http.service.result

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
    } catch (e: APIException) {
        APIResult.failure(e)
    }
}