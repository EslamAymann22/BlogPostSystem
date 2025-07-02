using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Service.Features.Role.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.APIs.Controllers
{

    public class RoleController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPut("ChangeUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse<string>>> ChangeUserRole([FromBody] ChangeUserRoleModel Model)
        {
            return Ok(await _mediator.Send(Model));
        }

    }
}
