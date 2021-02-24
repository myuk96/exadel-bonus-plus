using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExadelBonusPlus.WebApi.Installers
{
    public class CollectionsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddBonusTransient();
            services.AddHistoryTransient();
            services.AddApiIdentityConfiguration(configuration);
            services.AddVendorTransient();
        }
    }
}
