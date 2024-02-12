using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SubscriptionManager.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app,IRetryService retryService)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using SubscriptionManagerDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<SubscriptionManagerDbContext>();

            retryService.ExecuteWithRetryAsync(()=>dbContext.Database.MigrateAsync());
        }
    }
}
