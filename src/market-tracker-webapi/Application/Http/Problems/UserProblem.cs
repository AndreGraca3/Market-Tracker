using market_tracker_webapi.Application.Service.Errors.User;

namespace market_tracker_webapi.Application.Http.Problems;

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

    public class UserByIdNotFound(UserFetchingError.UserByIdNotFound data) : UserProblem(
            404,
            "user-not-found",
            "User not found",
            $"User with id {data.Id} not found",
            data
        );

    public class UserAlreadyExists(UserCreationError.CredentialAlreadyInUse data) : UserProblem(
        409,
        "user-already-exists",
        "User already exists",
        $"The {data.CredentialName} {data.CredentialValue} is already in use",
        data
    );
    
    public class DeviceTokenNotFound(UserFetchingError.DeviceTokenNotFound data) : UserProblem(
        404,
        "device-token-not-found",
        "Device token not found",
        $"Device token with client id {data.ClientId} and device id {data.DeviceId} not found",
        data
    );
    
    public static UserProblem FromServiceError(IUserError error)
        => error switch
        {
            UserCreationError.InvalidEmail invalidEmail => new InvalidEmail(invalidEmail),
            UserFetchingError.UserByIdNotFound userByIdNotFound => new UserByIdNotFound(userByIdNotFound),
            UserCreationError.CredentialAlreadyInUse userAlreadyExists => new UserAlreadyExists(userAlreadyExists),
            UserFetchingError.DeviceTokenNotFound deviceTokenNotFound => new DeviceTokenNotFound(deviceTokenNotFound),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
}