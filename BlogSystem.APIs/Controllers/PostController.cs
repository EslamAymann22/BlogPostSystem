using BlogSystem.APIs.Helper;
using BlogSystem.Core.Entities;
using BlogSystem.Service.Features.Posts.Command;
using BlogSystem.Service.Features.Posts.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.APIs.Controllers
{

    public class PostController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<GetPostsDto>>> GetAllPostsAsync(GetAllPostsModel model)
        {
            return Ok(_mediator.Send(model));

        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost("CreatePost")]
        public async Task<ActionResult<string>> CreateNewPost(CreatePostModel model)
        {
            return Ok(_mediator.Send(model));
        }

        [HttpDelete("DeletePost/{Id}")]
        [Authorize(Roles = ("Admin,Editor"))]
        public async Task<ActionResult<GetPostsDto>> DeletePost(int Id)
        {
            var Model = new DeletePostModel { PostId = Id, UserEmail = User.FindFirstValue(ClaimTypes.Email) };
            return Ok(await _mediator.Send(Model));
        }

        [Authorize]
        [HttpGet("Post/{id}")]
        public async Task<ActionResult<GetPostsDto>> GetPostById(int id)
        {
            var Model = new GetPostByIdModel { PostId = id, UserEmail = User.FindFirstValue(ClaimTypes.Email) };
            return Ok(await _mediator.Send(Model));
        }

        [HttpPut("UpdatePost")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Post>> UpdatePostData(UpdatePostModel model)
        {

            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _mediator.Send(model));
        }
    }
}
