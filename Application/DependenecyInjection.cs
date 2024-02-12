using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependenecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddMediatR(config =>
                    config.RegisterServicesFromAssemblies(typeof(DependenecyInjection).Assembly));

       

            return services;
        }
    }
}
