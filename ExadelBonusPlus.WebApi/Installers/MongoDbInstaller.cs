using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExadelBonusPlus.WebApi.Installers
{
    public class MongoDbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection(
               nameof(MongoDbSettings)));
        }
    }
}
