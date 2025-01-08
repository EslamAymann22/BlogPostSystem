using AutoMapper;
using BlogSystem.APIs.DTOs;
using BlogSystem.APIs.Errors;
using BlogSystem.APIs.Helper;
using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Repositories;
using BlogSystem.Core.Specifications;
using BlogSystem.Core.Specifications.Params;
using BlogSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BlogSystem.APIs.Controllers
{

    public class CommentController : ApiBaseController
    {
        private readonly IGenericRepository<Comment> _CommentRepo;
        private readonly IGenericRepository<Post> _postRepo;
        private readonly UserManager<AppUser> _manager;
        private readonly IMapper _mapper;

        public CommentController(IGenericRepository<Comment> CommentRepo
                                 , IGenericRepository<Post> PostRepo
                                 , UserManager<AppUser> userManager
                                 , IMapper mapper)
        {
            _CommentRepo = CommentRepo;
            _postRepo = PostRepo;
            _manager = userManager;
            _mapper = mapper;
        }

        [HttpPost("CreateComment")]
        [Authorize]
        public async Task<ActionResult<CommentDto>> CreateComment(CommentDto model)
        {
            var _Post = await _postRepo.GetByIdAsync(model.PostId);
            if (_Post.Status != PostStatus.Published)
                return BadRequest(new ApiResponse(404, "This post is not found"));

            var _Email = User.FindFirstValue(ClaimTypes.Email);
            var _Account = await _manager.FindByEmailAsync(_Email);
            var _NewComment = new Comment()
            {
                AuthorId = _Account.Id,
                Content = model.Content,
                CreatedAt = DateTime.UtcNow,
                PostId = model.PostId
            };
            model.AuthorName = _Account.DisplayName;
            await _CommentRepo.AddAsync(_NewComment);
            return Ok(model);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CommentDto>> GetCommentById(int id)
        {
            var Spe = new CommentSpecificationWithAuthInclude(id);

            var _Comment = await _CommentRepo.GetByIdSpecAsync(Spe);

            if(_Comment is null)
                return NotFound(new ApiResponse(404, "This comment is not found"));

            var _ReturnedComment = new CommentDto()
            {
                AuthorName = _Comment.Author.DisplayName,
                Content = _Comment.Content,
                CreatedAt = _Comment.CreatedAt,
                PostId = id
            };
            return Ok(_ReturnedComment);
        }

        [HttpGet("Post/{id}")]
        [Authorize]
        public async Task<ActionResult<Pagination<CommentDto>>> GetCommentByPostId(int id, [FromQuery] CommentSpecificationParams Params)
        {
            var _Post = await _postRepo.GetByIdAsync(id);
            if (_Post.Status != PostStatus.Published)
                return BadRequest(new ApiResponse(404, "This post is not found"));

            var Spe = new CommentSpecificationWithAuthInclude(Params, id);
            var _AllComments = await _CommentRepo.GetAllSpecAsync(Spe);
            var CommentsDtos = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(_AllComments);
            //return Ok(CommentsDtos);
            return Ok(new Pagination<CommentDto>(Spe.take, Params.Index, Spe.countOfElements, CommentsDtos));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<Tuple<CommentDto,string>> DeleteComment(int id)
        {
            // Get the comment
            var _Comment = _GetCommentByIdAsync(id);
            if (_Comment.Result is null)
                return NotFound(new ApiResponse(404, "This comment is not found"));

            // get user Email
            var _UserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (!User.IsInRole("Admin") && _Comment.Result.Author.Email != _UserEmail)
                return Unauthorized(new ApiResponse(401, "You not have permutation to delete this comment"));

            // Delete comment
            _CommentRepo.Delete(_Comment.Result);
            var _DeletedComment = _mapper.Map<Comment,CommentDto>(_Comment.Result);

            // Create a cool message for user 
            var CommentWithMessage = new
            {
                DeletedComment = _DeletedComment,
                Message = "Deleted Successfully"
            };
            return Ok(CommentWithMessage);

        }

        private async Task<Comment> _GetCommentByIdAsync(int id)
        {
            var Spe = new CommentSpecificationWithAuthInclude(id);

            var _Comment = await _CommentRepo.GetByIdSpecAsync(Spe);

            return _Comment;
        }

    }
}
