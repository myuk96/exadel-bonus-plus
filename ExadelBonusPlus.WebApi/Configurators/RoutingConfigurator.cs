using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace ExadelBonusPlus.WebApi.Configurators
{
    public class RoutingConfigurator : IConfigurator
    {
        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseRouting();
        }
    }
}
