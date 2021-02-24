using ExadelBonusPlus.WebApi.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace ExadelBonusPlus.WebApi.Configurators
{
    public class SwaggerConfigurator : IConfigurator
    {
        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            var swaggerOptions = new SwaggerOptions();
            configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerOptions.JsonRoute;
            });

            app.UseSwaggerUI(options => 
                {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Exadel Bonus Plus API 1.0");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Exadel Bonus Plus API 2.0");
                }
            );
        }
    }
}
