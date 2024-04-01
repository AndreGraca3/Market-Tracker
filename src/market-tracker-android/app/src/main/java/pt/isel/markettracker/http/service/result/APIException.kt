package pt.isel.markettracker.http.service.result

import pt.isel.markettracker.http.problem.Problem

data class APIException(val problem: Problem): Throwable(problem.detail) // TODO: discuss this