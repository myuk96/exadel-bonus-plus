using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ExadelBonusPlus.WebApi.Configurators
{
    interface IConfigurator
    {
        void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration);
    }
}
