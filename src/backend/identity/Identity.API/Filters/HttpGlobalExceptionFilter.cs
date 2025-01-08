namespace Identity.API.Filters;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HttpGlobalExceptionFilter> logger;

    public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        logger.LogError(new EventId(context.Exception.HResult),
            context.Exception,
            context.Exception.Message);

        var problemDetails = new ValidationProblemDetails()
        {
            Instance = context.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Please refer to the errors property for additional details."
        };

        problemDetails.Errors.Add("Exceptions", new string[] { GetInnerExceptionMessage(context.Exception) });

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