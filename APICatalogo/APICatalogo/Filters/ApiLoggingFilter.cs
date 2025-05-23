﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Executa antes do action
        _logger.LogInformation("### Executando -> onActionExecuting");
        _logger.LogInformation("##############################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
        _logger.LogInformation("##############################################");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // executa apos o action
        _logger.LogInformation("### Executando -> onActionExecuted");
        _logger.LogInformation("##############################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Status Code : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("##############################################");
    }
}
