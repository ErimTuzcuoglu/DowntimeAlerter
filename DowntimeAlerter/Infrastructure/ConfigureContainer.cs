using DowntimeAlerter.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace DowntimeAlerter.Infrastructure
{
    public class ConfigureContainer
    {
        public static void AddCustomExceptionHandler(IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
        
        public static void AddLogger(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/{Date}.txt");
        }
        
        public static void AddSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Downtime Alerter");
                c.RoutePrefix = "swagger";
            });
        }
    }
}