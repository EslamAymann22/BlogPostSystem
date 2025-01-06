using AutoMapper;
using BlogSystem.APIs.DTOs;
using BlogSystem.APIs.Errors;
using BlogSystem.APIs.Helper;
using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Repositories;
using BlogSystem.Core.Specifications;
using BlogSystem.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.APIs.Controllers
{
    
    public class PostController : ApiBaseController
    {
        private readonly IGenericRepository<Post> _blogPosts;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public PostController(IGenericRepository<Post> blogPosts
                             ,IMapper mapper
                             ,UserManager<AppUser> userManager )
        {
            _blogPosts = blogPosts;
            _mapper = mapper;
            this._userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<PostDtoToReturn>>> GetAllPostsAsync([FromQuery] PostSpecificationParams Parms )
        {
            //if(Parms.status == PostStatus.Published) { Parms.status = null; }

            var _CutAccount = await _GetCurrentUser();

            if (Parms.status is not null && _CutAccount.Role != UserRole.Admin && Parms.status != PostStatus.Published)
                return NotFound(new ApiResponse(401, "Not Allowed For U To Use This Feature Search"));

            if (Parms.status == null && _CutAccount.Role != UserRole.Admin)
                Parms.status = PostStatus.Published;

            var Spec = new PostSpecificationWithAllIncludes(Parms);
            var Result = await _blogPosts.GetAllSpecAsync(Spec);
            var ResultDto = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDtoToReturn>>(Result);

            ///var Result = await _blogPosts.GetAllAsync();
            ///return Ok(ResultDto);
            ///return Ok(Result);

            return Ok(new Pagination<PostDtoToReturn>(Spec.take, Parms.index, Spec.countOfElements, ResultDto));

        }


        [Authorize]
        [HttpGet("{id}")]
        // BaseUrl/Api/post/id(int)

        public async Task<ActionResult<PostDtoToReturn>> GetPostByIdAsync(int id)
        {
            var Spec = new PostSpecificationWithAllIncludes(id);
            //var Result =await _blogPosts.GetByIdAsync(id);
            var Result =await _blogPosts.GetByIdSpecAsync(Spec);

            if (Result == null) return BadRequest(new ApiResponse(404));

            var _CutAccount = await _GetCurrentUser();

            if (Result.Status!=PostStatus.Published && _CutAccount.Role!=UserRole.Admin)
                return NotFound(new ApiResponse(401, "Not Allowed For U To Use This Feature Search"));

            var ResultDto = _mapper.Map<Post, PostDtoToReturn>(Result);

            return Ok(ResultDto);
        }

        //[HttpPost]
        //public async Task<ActionResult<PostDtoToReturn>> CreateNewPost(PostDtoToReturn model)
        //{
        //    var _Account = await _GetCurrentUser();
        //    if(_Account.Role == UserRole.Reader) return NotFound(new ApiResponse(401,"You are don't allow to create new post"));

        //    User.IsInRole("Admin");

        //    try
        //    {

        //        Post _NewPost = new Post()
        //        {
        //            Id = model.Id,
        //            Title = model.Title,
        //            Content = model.Content,
        //            Author = _
        //        }

        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ex);
        //        return BadRequest(new ApiResponse(500, ex.Message));

        //    }

        //}

        [Authorize]
        private async Task<AppUser> _GetCurrentUser()
        {
            var _Email = User.FindFirstValue(ClaimTypes.Email);
            var _Account = await _userManager.FindByEmailAsync(_Email);

            return _Account;
        }


    }
} 
