using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeAlerter.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowntimeAlerter.Infrastructure.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            _logger.LogError(e.Message);
            var message = JsonConvert.DeserializeObject<IEnumerable<string>>(e.Message);

            // context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var response = new ApiResponse<string>
            {
                Message = "Error",
                Errors = message.ToList()
            };
            var camelSettings = new JsonSerializerSettings
                {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var json = JsonConvert.SerializeObject(response, camelSettings);
            await context.Response.WriteAsync(json);
        }
    }
}