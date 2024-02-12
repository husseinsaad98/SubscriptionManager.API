using Application.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UserRepository(SubscriptionManagerDbContext context, SignInManager<User> signInManager, UserManager<User> userManager) : base(context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await Context.Set<User>().FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower(), cancellationToken);
        }

        public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<User>().FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return !(await Context
             .Set<User>()
             .AnyAsync(user => user.Email == email, cancellationToken));
        }
        public async Task<IdentityResult> Add(User user)
        {
           return await _userManager.CreateAsync(user, user.PasswordHash);
        }

        public async Task<bool> IsUserAuthenticated(string email, string password)
        {

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
            return result.Succeeded;
        }
        public async Task<IEnumerable<string>> GetUserClaims(User user, CancellationToken cancellationToken = default)
        {
            var claims = await _userManager.GetRolesAsync(user);
            return claims;
        }
    }
}