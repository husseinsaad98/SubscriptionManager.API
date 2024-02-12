using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class SubscriptionManagerDbContext : IdentityDbContext<User>
    {
        public SubscriptionManagerDbContext(DbContextOptions options) : base(options)
        {

        }
        public override DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionTypes { get; set; }
        public DbSet<SubscriptionStatus> SubscriptionStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithOne(u => u.Subscription)
                .HasForeignKey<User>(x => x.SubscriptionId);

            modelBuilder.Entity<SubscriptionPlan>()
              .HasMany(s => s.Subscriptions)
              .WithOne(u => u.SubscriptionType)
              .HasForeignKey(x => x.SubscriptionTypeId);

            modelBuilder.Entity<SubscriptionStatus>()
            .HasMany(s => s.Subscriptions)
            .WithOne(u => u.SubscriptionStatus)
            .HasForeignKey(x => x.SubscriptionStatusId);

            modelBuilder.Entity<User>()
             .HasOne(s => s.Subscription)
             .WithOne(u => u.User)
             .HasForeignKey<Subscription>(x => x.UserId);


            //fill data on migration create admin user and other data
            new DbInitializer(modelBuilder).Seed();
        }
    }
}
