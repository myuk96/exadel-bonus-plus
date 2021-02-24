using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ExadelBonusPlus.WebApi.Configurators
{
    public class AuthConfigurator : IConfigurator
    {
        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
