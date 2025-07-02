using BlogSystem.Core.ResponseBase.Paginations;
using BlogSystem.Service.Features.Tags.Command;
using BlogSystem.Service.Features.Tags.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> CreateTag([FromBody] CreateTagModel model)
        {
            return Ok(await _mediator.Send(model));
        }

        public async Task<ActionResult<PaginatedResponse<GetAllTagsDto>>> GetAllTag([FromBody] GetAllTagsModel model)
        {
            return Ok(await _mediator.Send(model));
        }

    }
}
