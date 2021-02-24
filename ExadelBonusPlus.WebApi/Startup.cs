using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExadelBonusPlus.WebApi.Installers;
using ExadelBonusPlus.WebApi.Configurators;

namespace ExadelBonusPlus.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfiguratorExtensions.ConfigureServicesInAssembly(app, env, _configuration);
         }
    }
}
