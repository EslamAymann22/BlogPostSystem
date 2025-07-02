using BlogSystem.APIs.Helper;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Service.Features.Comments.Command;
using BlogSystem.Service.Features.Comments.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.APIs.Controllers
{

    public class CommentController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("CreateComment")]
        [Authorize]
        public async Task<ActionResult<string>> CreateComment(CreateCommentModel model)
        {
            return Ok(await _mediator.Send(model));
        }

        [HttpGet("Comment/{id}")]
        public async Task<ActionResult<BaseResponse<GetCommentDto>>> GetCommentById(int id)
        {

            var Model = new GetCommentByIdModel() { CommentId = id };
            return Ok(await _mediator.Send(Model));
        }

        [HttpGet("Comments")]
        [Authorize]
        public async Task<ActionResult<Pagination<GetCommentDto>>> GetCommentByPostId(int id, [FromQuery] GetAllCommentsByPostIdModel Model)
        {
            return Ok(await _mediator.Send(Model));
        }

        [HttpDelete("DeleteCommentById")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> DeleteCommentById(DeleteCommentByIdModel Model)
        {
            Model.UserEmal = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _mediator.Send(Model));
        }


    }
}
