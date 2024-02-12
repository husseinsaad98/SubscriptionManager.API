using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(SubscriptionManagerDbContext context) : base(context)
        {
        }
        public async Task<Subscription> GetSubscriptionByUserId(string userId)
        {
           return await Context.Subscriptions.FirstOrDefaultAsync(x=>x.UserId  == userId);
        }

     
    }
}