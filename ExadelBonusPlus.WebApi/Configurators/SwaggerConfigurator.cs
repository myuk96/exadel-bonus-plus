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
                options.SwaggerEndpoint
                (swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });
        }
    }
}
