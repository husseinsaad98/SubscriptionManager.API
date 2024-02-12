using Domain.Models;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(SubscriptionManagerDbContext context) : base(context)
        {
        }
    }
}
