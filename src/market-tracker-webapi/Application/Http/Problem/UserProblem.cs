using market_tracker_webapi.Application.Service.Errors.User;

namespace market_tracker_webapi.Application.Http.Problem;

public class UserProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class InvalidEmail(UserCreationError.InvalidEmail data) : UserProblem(
        400,
        "invalid-email",
        "Invalid email",
        $"The email {data.Email} is invalid",
        data
    );

    public class UserByIdNotFound(UserFetchingError.UserByIdNotFound data)
        : UserProblem(
            404,
            "user-not-found",
            "User not found",
            $"User with id {data.Id} not found",
            data
        );

    public class UserAlreadyExists(UserCreationError.EmailAlreadyInUse data) : UserProblem(
        409,
        "user-already-exists",
        "User already exists",
        $"The email {data.Email} is already in use",
        data
    );
}