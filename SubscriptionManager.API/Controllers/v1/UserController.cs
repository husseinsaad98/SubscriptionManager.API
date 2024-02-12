using Application.CQRS.User.Commands;
using Application.CQRS.User.Queries;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManager.API.Identity;

namespace SubscriptionManager.API.Controllers.v1
{
    public class UserController : BaseApiController
    {
        [HttpGet]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> GetUsers()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser([FromBody] UserCredentials userCredentials)
        {
            return HandleResult(await Mediator.Send(new Login.Command { UserCredentials = userCredentials }));
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            return HandleResult(await Mediator.Send(new Create.Command { User = userDto }));
        }
        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            return HandleResult(await Mediator.Send(new GetById.Query { UserId = UserId }));
        }
        [HttpGet]
        [Route("GetUserInfoById/{useriId}")]
        [Authorize(policy: IdentityData.AdminPolicyKey)]
        public async Task<IActionResult> GetUserInfoById(string useriId)
        {
            return HandleResult(await Mediator.Send(new GetById.Query { UserId = useriId }));
        }
    }
}
