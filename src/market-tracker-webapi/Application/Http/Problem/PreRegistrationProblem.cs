using market_tracker_webapi.Application.Service.Errors.PreRegister;

namespace market_tracker_webapi.Application.Http.Problem;

public class PreRegistrationProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class PreRegistrationByIdNotFound(PreRegistrationFetchingError.PreRegistrationByIdNotFound data)
        : UserProblem(
            404,
            "operator-not-found",
            "Operator not found",
            $"Registration with id {data.Id} not found",
            data
        );

    public class PreRegistrationNotValidated(PreRegistrationFetchingError.PreRegistrationNotValidated data)
        : UserProblem(
            403,
            "registration-not-valid",
            "Registration not valid",
            $"Registration not valid yet, try again later.",
            data
        );

    public class OperatorAlreadyExists(PreRegistrationCreationError.EmailAlreadyInUse data) : UserProblem(
        409,
        "operator-already-exists",
        "Operator already exists",
        $"The Email {data.Email} is already in use",
        data
    );
}