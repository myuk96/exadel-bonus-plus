using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExadelBonusPlus.WebApi.Installers
{
    public class ControllersInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilterAttribute));
                options.Filters.Add(typeof(ValidationFilterAttribute));
                options.Filters.Add(typeof(HttpModelResultFilterAttribute));
            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true; //This string is needed for fluent validation in action filter

                })
                .AddFluentValidation();
        }
    }
}
