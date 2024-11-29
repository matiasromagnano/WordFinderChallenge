using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WordFinderChallenge.API.Models;

namespace WordFinderChallenge.API.Filters;

public class NormalizeApiResponseAttribute : ActionFilterAttribute
{
    private const string Success = nameof(Success);

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var result = context.Result;

        if (result is ObjectResult objectResult && objectResult.Value != null)
        {
            var statusCode = objectResult.StatusCode ?? 200;
            var responseBody = objectResult.Value;
            var normalizedResponse = new ApiResponse<object>();

            if (statusCode is StatusCodes.Status400BadRequest)
            {
                //Getting the ProblemDetails from the ASP.NET Built-in Middleware
                var problemDetails = (ValidationProblemDetails)responseBody;
                var errors = problemDetails.Errors;
                if (errors is not null)
                {
                    normalizedResponse.StatusCode = statusCode;
                    normalizedResponse.Message = problemDetails.Title;
                    normalizedResponse.Details = errors;
                    normalizedResponse.Data = null;
                }
            }
            else
            {
                normalizedResponse.StatusCode = statusCode;
                normalizedResponse.Data = responseBody;
                normalizedResponse.Message = Success;
            }

            context.Result = new ObjectResult(normalizedResponse)
            {
                StatusCode = statusCode,
                DeclaredType = typeof(ApiResponse<object>)
            };
        }
        else if (result is StatusCodeResult statusCodeResult)
        {
            var statusCode = statusCodeResult.StatusCode;

            var normalizedResponse = new ApiResponse<object>
            {
                StatusCode = statusCode,
                Message = Success
            };

            context.Result = new ObjectResult(normalizedResponse)
            {
                StatusCode = statusCode,
                DeclaredType = typeof(ApiResponse<object>)
            };
        }

        base.OnResultExecuting(context);
    }
}
