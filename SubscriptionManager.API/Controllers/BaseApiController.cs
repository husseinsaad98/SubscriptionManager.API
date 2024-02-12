using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SubscriptionManager.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseApiController : ControllerBase
    {
        public string UserId
        {
            get
            {
                if (User != null)
                {
                    var userIdNameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdNameClaim != null)
                    {
                        return userIdNameClaim.Value;
                    }
                }

                return null;
            }
        }
        protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>()!;
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
                return NotFound();

            //we can return noContent if the value is null
            //if (result.IsSuccess && result.Value == null)
            //    return NoContent();

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }


    }

}
