using Domain.Models;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class SubscriptionStatusReposiotory : Repository<SubscriptionStatus>, ISubscriptionStatusRepository
    {
        public SubscriptionStatusReposiotory(SubscriptionManagerDbContext context) : base(context)
        {
        }
    }
}
