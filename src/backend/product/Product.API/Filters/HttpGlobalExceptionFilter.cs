using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Product.API.Filters;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var problemDetails = new ValidationProblemDetails()
        {
            Instance = context.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Please refer to the errors property for additional details."
        };

        problemDetails.Errors.Add("Errors", new string[] { GetInnerExceptionMessage(context.Exception) });

        context.Result = new BadRequestObjectResult(problemDetails);
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }

    private string GetInnerExceptionMessage(Exception ex)
    {
        string result = null;

        if (ex.InnerException == null)
        {
            result = ex.Message;
        }
        else
        {
            result = GetInnerExceptionMessage(ex.InnerException);
        }
        return result;
    }
}
