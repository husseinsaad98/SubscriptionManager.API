using Application.CQRS.SubscriptionStatus.Commands;
using Application.CQRS.SubscriptionStatus.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManager.API.Identity;

namespace SubscriptionManager.API.Controllers.v1
{

    public class SubscriptionStatusController : BaseApiController
    {
        [HttpGet]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> GetAllStatus()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }
        [HttpPost]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        [Route("CreateSubscriptionStatus")]
        public async Task<IActionResult> CreateSubscriptionStatus([FromBody] SubscriptionStatus subscriptionStatus)
        {
            return HandleResult(await Mediator.Send(new Create.Command { SubscriptionStatus = subscriptionStatus }));
        }
        [HttpDelete]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        [Route("DeleteSubscriptionStatus/{subscriptionStatusId}")]
        public async Task<IActionResult> DeleteSubscriptionPlan(int subscriptionStatusId)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { SubscriptionStatusId = subscriptionStatusId }));
        }

    }
}
