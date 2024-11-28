using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WordFinderChallenge.API.Models;
using WordFinderChallenge.Core.Exceptions;

namespace WordFinderChallenge.API.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case NotFoundException notFoundException:
                HandleNotFoundException(context, notFoundException);
                break;
            case BadRequestException badRequestException:
                HandleBadRequestException(context, badRequestException);
                break;
            default:
                HandleOtherExceptions(context);
                break;
        }
    }

    private static void HandleNotFoundException(ExceptionContext context, NotFoundException exception)
    {
        context.Result = new ObjectResult(new ApiResponse<NotFoundException>
        {
            StatusCode = exception.StatusCode,
            Message = exception.Message,
            Data = null
        })
        {
            StatusCode = StatusCodes.Status404NotFound
        };

        context.ExceptionHandled = true;
    }

    private static void HandleBadRequestException(ExceptionContext context, BadRequestException exception)
    {
        context.Result = new ObjectResult(new ApiResponse<BadRequestException>
        {
            StatusCode = exception.StatusCode,
            Message = exception.Message,
            Data = null
        })
        {
            StatusCode = StatusCodes.Status400BadRequest
        };

        context.ExceptionHandled = true;
    }

    private static void HandleOtherExceptions(ExceptionContext context)
    {
        string? message;
        if (context.Exception.InnerException is not null)
        {
            message = context.Exception.InnerException.Message;
        }
        else
        {
            message = context.Exception.Message;
        }

        context.Result = new ObjectResult(new ApiResponse<OtherException>
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = message,
            Data = null
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
