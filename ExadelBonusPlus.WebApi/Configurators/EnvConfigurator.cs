using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ExadelBonusPlus.WebApi.Configurators
{
    public class EnvConfigurator : IConfigurator
    {
        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 5
                };
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }
            app.UseStaticFiles();
        }
    }
}
