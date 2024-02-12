using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrustructure(this IServiceCollection services, IConfiguration config)
        {
            // adding DbContext
            services.AddDbContext<SubscriptionManagerDbContext>(opt =>
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"))
            );

            return services;
        }
    }
}
