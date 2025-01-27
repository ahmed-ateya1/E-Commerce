using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.UnitOfWorkConfig;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure.DependencyInjectionConfig
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
