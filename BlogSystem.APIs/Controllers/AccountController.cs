using BlogSystem.APIs.DTOs;
using BlogSystem.Service.Features.Accounts.Command;
using BlogSystem.Service.Features.Accounts.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.APIs.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly Mediator _mediator;

        public AccountController(Mediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginModel Model)
        {
            return Ok(await _mediator.Send(Model));
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterModel Model)
        {
            return Ok(await _mediator.Send(Model));
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurUser()
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            var Model = new GetCurUserModel { Email = UserEmail };

            return Ok(await _mediator.Send(Model));
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = ("Admin"))]
        public async Task<ActionResult<string>> DeleteUser(DeleteUserModel Model)
        {
            return Ok(await _mediator.Send(Model));
        }
    }
}
