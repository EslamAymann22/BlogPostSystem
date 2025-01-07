using AutoMapper;
using BlogSystem.APIs.DTOs;
using BlogSystem.APIs.Errors;
using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Repositories;
using BlogSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                                 ,IGenericRepository<Post> PostRepo
                                 ,UserManager<AppUser> userManager
                                 ,IMapper mapper)
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
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            var _Comment = await _CommentRepo.GetByIdAsync(id);
            //var _ReturnedComment = new CommentDto()
            //{
            //    AuthorName = _Comment.
            //}
            return Ok(_Comment);
        }

        [HttpGet("Post/{id}")]
        [Authorize]
        public async Task<ActionResult<List<CommentDto>>> GetCommentByPostId(int id)
        {
            var _Post =await _postRepo.GetByIdAsync(id);
            if (_Post.Status != PostStatus.Published)
                return BadRequest(new ApiResponse(404, "This post is not found"));

            var _AllComments = await _CommentRepo.GetAllAsync();
            var CommentsDtos = _mapper.Map<List<CommentDto>>(_AllComments);
            return Ok(CommentsDtos);
        }


    }
}
