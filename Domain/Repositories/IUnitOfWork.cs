namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISubscriptionRepository Subscriptions { get; }
        ISubscriptionPlanRepository SubscriptionPlans { get; }
        ISubscriptionStatusRepository SubscriptionStatus { get; }
        Task<int> CompleteAsync();
    }
}
