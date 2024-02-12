using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<SubscriptionPlan>().HasData(
                   new SubscriptionPlan() { Id = 1, Name = "Basic" },
                   new SubscriptionPlan() { Id = 2, Name = "Standard" },
                   new SubscriptionPlan() { Id = 3, Name = "Premium" }
            );

            modelBuilder.Entity<SubscriptionStatus>().HasData(
                   new SubscriptionPlan() { Id = 1, Name = "Active" },
                   new SubscriptionPlan() { Id = 2, Name = "Expired" },
                   new SubscriptionPlan() { Id = 3, Name = "Canceled" }
            );


            string ADMIN_ID = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            string ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

            //seed admin role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            //create user
            var appUser = new User
            {
                Id = ADMIN_ID,
                Email = "mounir@gmail.com",
                EmailConfirmed = true,
                FirstName = "Mounir",
                PhoneNumber = "70982311",
                LastName = "Badran",
                UserName = "mounir@gmail.com",
                NormalizedUserName = "MOUNIR@GMAIL.COM"
            };

            //set user password
            PasswordHasher<User> ph = new();
            appUser.PasswordHash = ph.HashPassword(appUser, "P@ssw0rd");

            //seed user
            modelBuilder.Entity<User>().HasData(appUser);

            //set user role to admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }

    }

}
