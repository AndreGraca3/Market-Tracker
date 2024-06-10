using market_tracker_webapi.Application.Http.Problems;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Alert;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Errors.Token;
using market_tracker_webapi.Application.Service.Errors.User;
using Microsoft.AspNetCore.Diagnostics;

namespace market_tracker_webapi.Application.Http.Pipeline;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is MarketTrackerServiceException serviceException)
        {
            Problem problem = serviceException.ServiceError switch
            {
                IAlertError alertError => AlertProblem.FromServiceError(alertError),
                ICategoryError categoryError => CategoryProblem.FromServiceError(categoryError),
                ICityError cityError => CityProblem.FromServiceError(cityError),
                ICompanyError companyError => CompanyProblem.FromServiceError(companyError),
                IListError listError => ListProblem.FromServiceError(listError),
                IListEntryError listEntryError => ListEntryProblem.FromServiceError(listEntryError),
                IPreRegistrationError preRegistrationError => PreRegistrationProblem.FromServiceError(preRegistrationError),
                IProductError productError => ProductProblem.FromServiceError(productError),
                IStoreError storeError => StoreProblem.FromServiceError(storeError),
                IUserError userError => UserProblem.FromServiceError(userError),
                ITokenError tokenError => TokenProblem.FromServiceError(tokenError),
                IGoogleTokenError googleTokenError => GoogleProblem.FromServiceError(googleTokenError),
                _ => new ServerProblem.InternalServerError()
            };
            
            Console.WriteLine(problem);

            httpContext.Response.StatusCode = problem.Status;

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ServerProblem.InternalServerError(exception.Message)
        );

        return true;
    }
}
