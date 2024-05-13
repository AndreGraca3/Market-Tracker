using market_tracker_webapi.Application.Service.Errors.PreRegister;

namespace market_tracker_webapi.Application.Http.Problems;

public class PreRegistrationProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class PreRegistrationByIdNotFound(PreRegistrationFetchingError.PreRegistrationByIdNotFound data)
        : PreRegistrationProblem(
            404,
            "operator-not-found",
            "Operator not found",
            $"Registration with id {data.Id} not found",
            data
        );

    public class PreRegistrationNotValidated(PreRegistrationFetchingError.PreRegistrationNotValidated data)
        : PreRegistrationProblem(
            403,
            "registration-not-valid",
            "Registration not valid",
            "Registration not valid yet, try again later.",
            data
        );

    public class OperatorAlreadyExists(PreRegistrationCreationError.EmailAlreadyInUse data) : PreRegistrationProblem(
        409,
        "operator-already-exists",
        "Operator already exists",
        $"The Email {data.Email} is already in use",
        data
    );

    public static PreRegistrationProblem FromServiceError(IPreRegistrationError error)
    {
        return error switch
        {
            PreRegistrationFetchingError.PreRegistrationByIdNotFound preRegistrationByIdNotFound =>
                new PreRegistrationByIdNotFound(preRegistrationByIdNotFound),
            PreRegistrationFetchingError.PreRegistrationNotValidated preRegistrationNotValidated =>
                new PreRegistrationNotValidated(preRegistrationNotValidated),
            PreRegistrationCreationError.EmailAlreadyInUse emailAlreadyInUse => new OperatorAlreadyExists(
                emailAlreadyInUse),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}