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
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogSystem.APIs.Controllers
{
    
    public class PostController : ApiBaseController
    {
        private readonly IGenericRepository<Post> _blogPosts;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly DbContextIdentity _dbContextIdentity;

        public PostController(IGenericRepository<Post> blogPosts
                             ,IMapper mapper
                             ,UserManager<AppUser> userManager
                             ,DbContextIdentity dbContextIdentity)
        {
            _blogPosts = blogPosts;
            _mapper = mapper;
            this._userManager = userManager;
            this._dbContextIdentity = dbContextIdentity;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<PostDtoToReturn>>> GetAllPostsAsync([FromQuery] PostSpecificationParams Parms )
        {
            //if(Parms.status == PostStatus.Published) { Parms.status = null; }

            //var _CutAccount = await _GetCurrentUser();

            //if (Parms.status is not null && _CutAccount.Role != UserRole.Admin && Parms.status != PostStatus.Published)
            if (Parms.status is not null && !User.IsInRole("Admin") && Parms.status != PostStatus.Published)
                return NotFound(new ApiResponse(401, "Not Allowed For U To Use This Feature Search"));

            if (Parms.status == null && !User.IsInRole("Admin"))
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
            var Result = await _blogPosts.GetByIdSpecAsync(Spec);

            if (Result == null) return BadRequest(new ApiResponse(404));


            //var _CutAccount = await _GetCurrentUser();
            //if (Result.Status!=PostStatus.Published && _CutAccount.Role!=UserRole.Admin)
            if (Result.Status!=PostStatus.Published && !User.IsInRole("Admin"))
                return NotFound(new ApiResponse(401, "Not Allowed For U To Use This Feature Search"));

            var ResultDto = _mapper.Map<Post, PostDtoToReturn>(Result);

            return Ok(ResultDto);
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public async Task<ActionResult<Post>> CreateNewPost(PostDtoToReturn model)
        {
            //var _Account = await _GetCurrentUser();
            //if (_Account.Role == UserRole.Reader)
                //return NotFound(new ApiResponse(401, "You are don't allow to create new post"));

            try
            {
                Post _NewPost = new Post()
                {
                    Title = model.Title,
                    Content = model.Content,
                    CreatedAt = DateTime.Now,
                    Status = PostStatus.Published
                };

                _NewPost.Tags = new List<Tag>();

                _NewPost.Tags =await _dbContextIdentity.tags
                    .Where(T=>model.Tags.Contains(T.Name))
                    .ToListAsync(); 

                ///foreach (var Tag in model.Tags)
                ///{
                ///    _NewPost.Tags.Add(await _dbContextIdentity.tags.Where(T => T.Name == Tag).FirstOrDefaultAsync());
                ///}


                _NewPost.Author =await _dbContextIdentity.Users.Where(U => U.DisplayName == model.Author).FirstOrDefaultAsync();
                _NewPost.AuthorId = _NewPost.Author.Id;
                _NewPost.Category =await _dbContextIdentity.categories.Where(U => U.Name == model.Category).FirstOrDefaultAsync();
                _NewPost.CategoryId = _NewPost.Category.Id;

                 await _dbContextIdentity.blogPosts.AddAsync(_NewPost);
                 await _dbContextIdentity.SaveChangesAsync();
                return _NewPost;
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500, ex.Message));

            }

        }

        [Authorize]
        private async Task<AppUser> _GetCurrentUser()
        {
            var _Email = User.FindFirstValue(ClaimTypes.Email);
            var _Account = await _userManager.FindByEmailAsync(_Email);

            return _Account;
        }


    }
} 
