using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SubscriptionManagerDbContext _context;

        public UnitOfWork(SubscriptionManagerDbContext context,SignInManager<User> signInManager,UserManager<User> userManager)
        {
            _context = context;
            Users = new UserRepository(_context, signInManager, userManager);
            Subscriptions = new SubscriptionRepository(_context);
            SubscriptionStatus = new SubscriptionStatusReposiotory(_context);
            SubscriptionPlans = new SubscriptionPlanRepository(_context);
        }

        public IUserRepository Users { get; private set; }

        public ISubscriptionRepository Subscriptions { get; private set; }

        public ISubscriptionPlanRepository SubscriptionPlans { get; private set; }

        public ISubscriptionStatusRepository SubscriptionStatus { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}