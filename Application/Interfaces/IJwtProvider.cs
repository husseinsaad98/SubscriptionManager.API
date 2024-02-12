using Domain.Models;
using System.Security.Claims;

namespace Application.Interfaces;

public interface IJwtProvider
{
    string Generate(User member, IList<string> roleClaims);
}
