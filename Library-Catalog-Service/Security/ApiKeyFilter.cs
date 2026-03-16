using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library_Catalog_Service.Security;

public class ApiKeyFilter : IActionFilter
{
    private readonly IConfiguration _config;

    public ApiKeyFilter(IConfiguration config)
    {
        _config = config;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-API-Key", out var key)
            || key != _config["ApiKey"])
        {
            context.Result = new UnauthorizedResult();
        }
    }
    public void OnActionExecuted(ActionExecutedContext context){ }
}