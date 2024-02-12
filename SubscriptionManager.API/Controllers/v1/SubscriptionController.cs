using Application.CQRS.Subscription.Commands;
using Application.CQRS.Subscription.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManager.API.Identity;
using static Domain.Enums.Subscription;

namespace SubscriptionManager.API.Controllers.v1
{
    [ApiController]
    public class SubscriptionController : BaseApiController
    {
        [HttpGet]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> Get()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }
        [HttpGet]
        [Route("GetMySubscription")]
        public async Task<IActionResult> GetMySubscription()
        {
            return HandleResult(await Mediator.Send(new GetByUserId.Query { UserId = UserId }));
        }
        [HttpGet]
        [Route("GetSubscriptionById/{subscriptionbId}")]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> GetSubscriptionById(int subscriptionbId)
        {
            return HandleResult(await Mediator.Send(new GetById.Query { SubscriptionId = subscriptionbId }));
        }
        [HttpGet]
        [Route("GetSubscriptiondByStatusId/{statusId}")]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> GetSubscriptionsByStatusId(int statusId)
        {
            return HandleResult(await Mediator.Send(new ListSubscriptionsByStatus.Query { StatusId = statusId }));
        }
        [HttpGet]
        [Route("GetSubscriptionRemainingDays/{subscriptionId}")]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> GetSubscriptionRemainingDays(int subscriptionId)
        {
            return HandleResult(await Mediator.Send(new GetSubscriptionRemainingDays.Query { SubscriptionId = subscriptionId }));
        }
        [HttpGet]
        [Route("GetMySubscriptionRemainingDays")]
        public async Task<IActionResult> GetMySubscriptionRemainingDays()
        {
            return HandleResult(await Mediator.Send(new GetMySubscriptionRemainingDays.Query { UserId = UserId }));
        }
        [HttpPost]
        [Route("CreateSubscription")]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> CreateSubscription([FromBody] Subscription subscription)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Subscription = subscription }));
        }

        [HttpPut]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        [Route("UpdateSubscriptionStatus/{subscriptionId}/{statusId}")]
        public async Task<IActionResult> UpdateSubscriptionStatus(int subscriptionId, int statusId)
        {
            return HandleResult(await Mediator.Send(new UpdateSubscriptionStatus.Command { SubscriptionId = subscriptionId, StatusId = statusId }));
        }
        [HttpDelete]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        [Route("DeleteScubscription/{subscriptionId}")]
        public async Task<IActionResult> DeleteScubscription(int subscriptionId)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { SubscriptionId = subscriptionId }));
        }
    }
}
