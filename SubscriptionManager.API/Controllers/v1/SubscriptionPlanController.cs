using Application.CQRS.SubscriptionPlan.Commands;
using Application.CQRS.SubscriptionPlan.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManager.API.Identity;

namespace SubscriptionManager.API.Controllers.v1
{
    public class SubscriptionPlanController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPlans()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }
        [HttpPost]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        [Route("CreateSubscriptionPlan")]
        public async Task<IActionResult> CreateSubscriptionPlan([FromBody] SubscriptionPlan subscriptionPlan)
        {
            return HandleResult(await Mediator.Send(new Create.Command { SubscriptionPlan = subscriptionPlan }));
        }
        [HttpDelete]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        [Route("DeleteSubscriptionPlan/{subscriptionPlanId}")]
        public async Task<IActionResult> DeleteSubscriptionPlan(int subscriptionPlanId)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { SubscriptionPlanId = subscriptionPlanId }));
        }
    }
}
