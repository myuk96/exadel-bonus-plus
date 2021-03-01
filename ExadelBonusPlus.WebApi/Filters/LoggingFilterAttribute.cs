using System;
using System.Linq;
using System.Threading.Tasks;
using ExadelBonusPlus.Services.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace ExadelBonusPlus.WebApi
{
    public class LoggongFilterAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly ILogger _logger;
        public LoggongFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context,
            ResourceExecutionDelegate next)
        {
            _logger.LogInformation($"{DateTime.Now} {context.HttpContext.Request.Method} {context.HttpContext.Request.Path} {context.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString()}");
            await next();
        }
    }
}
