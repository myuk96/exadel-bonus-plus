using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace ExadelBonusPlus.WebApi.Configurators
{
    public static class ConfiguratorExtensions
    {
        public static void ConfigureServicesInAssembly(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            var configurators = typeof(Startup).Assembly.ExportedTypes.Where(t => typeof(IConfigurator).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface).Select(Activator.CreateInstance).Cast<IConfigurator>().ToList();

            configurators.ForEach(configurator => configurator.ConfigureApp(app, env, configuration));
        }
    }
}
