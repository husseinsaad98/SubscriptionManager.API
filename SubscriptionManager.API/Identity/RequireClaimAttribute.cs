using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SubscriptionManager.API.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireClaimAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string claimName;
        private readonly string claimValue;

        public RequireClaimAttribute(string ClaimName, string ClaimValue)
        {
            claimName = ClaimName;
            claimValue = ClaimValue;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.HasClaim(claimName, claimValue))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
