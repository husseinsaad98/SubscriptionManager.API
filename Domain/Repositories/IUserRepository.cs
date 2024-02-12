using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetUserClaims(User user, CancellationToken cancellationToken = default);
        Task<bool> IsUserAuthenticated(string email, string password);

        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
        Task<IdentityResult> Add(User member);
    }

}
