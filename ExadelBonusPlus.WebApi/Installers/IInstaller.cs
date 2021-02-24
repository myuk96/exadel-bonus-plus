using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ExadelBonusPlus.WebApi.Installers
{
    interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
