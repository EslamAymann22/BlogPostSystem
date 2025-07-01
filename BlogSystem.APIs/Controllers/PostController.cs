using AutoMapper;
using BlogSystem.APIs.DTOs;
using BlogSystem.APIs.Errors;
using BlogSystem.APIs.Helper;
using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Repositories;
using BlogSystem.Core.Specifications;
using BlogSystem.Core.Specifications.Params;
using BlogSystem.Repository.Data;
using Microsoft.AspNetCore.Authorization;
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
        private readonly BlogPostDbContext _dbContextIdentity;

        public PostController(IGenericRepository<Post> blogPosts
                             , IMapper mapper
                             , UserManager<AppUser> userManager
                             , BlogPostDbContext dbContextIdentity)
        {
            _blogPosts = blogPosts;
            _mapper = mapper;
            this._userManager = userManager;
            this._dbContextIdentity = dbContextIdentity;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<PostDtoToReturn>>> GetAllPostsAsync([FromQuery] PostSpecificationParams Parms)
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
            if (Result.Status != PostStatus.Published && !User.IsInRole("Admin"))
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

                _NewPost.Tags = await _dbContextIdentity.tags
                    .Where(T => model.Tags.Contains(T.Name))
                    .ToListAsync();

                ///foreach (var Tag in model.Tags)
                ///{
                ///    _NewPost.Tags.Add(await _dbContextIdentity.tags.Where(T => T.Name == Tag).FirstOrDefaultAsync());
                ///}


                _NewPost.AuthorId = model.AuthorId;
                _NewPost.Author = await _dbContextIdentity.Users.Where(U => U.Id == model.AuthorId).FirstOrDefaultAsync();
                _NewPost.Category = await _dbContextIdentity.categories.Where(U => U.Name == model.Category).FirstOrDefaultAsync();
                _NewPost.CategoryId = _NewPost.Category.Id;

                await _blogPosts.AddAsync(_NewPost);
                return _NewPost;
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500, ex.Message));

            }

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = ("Admin"))]
        public async Task<ActionResult<PostDtoToReturn>> DeletePost(int Id)
        {

            var _Post = await _dbContextIdentity.blogPosts.Where(U => U.Id == Id).FirstOrDefaultAsync();
            if (_Post is null)
                return NotFound(new ApiResponse(404, "This Post id not found!!"));
            //var User =await _dbContextIdentity.Users.Where(U=>U.Id==Id).FirstOrDefaultAsync();

            try
            {
                await _blogPosts.DeleteAsync(_Post);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500, ex.Message));
            }


            return Ok(_mapper.Map<Post, PostDtoToReturn>(_Post));
        }


        [Authorize]
        private async Task<AppUser> _GetCurrentUser()
        {
            var _Email = User.FindFirstValue(ClaimTypes.Email);
            var _Account = await _userManager.FindByEmailAsync(_Email);

            return _Account;
        }

        private async Task<Post> _GetPostById(int id)
        {
            var Spec = new PostSpecificationWithAllIncludes(id);
            //var Result =await _blogPosts.GetByIdAsync(id);
            return await _blogPosts.GetByIdSpecAsync(Spec);


        }

        [HttpPut]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Post>> UpdatePostData(PostDtoUpdateModel model)
        {

            var _Post = await _GetPostById(model.Id);


            if (_Post is null)
                return BadRequest(new ApiResponse(404, "this post not founded"));
            var _UserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (!User.IsInRole("Admin") && _Post.Author.Email != _UserEmail)
                return BadRequest(new ApiResponse(401, "Not Allowed For U To Edit This Feature Post"));

            _Post.UpdatedAt = model.UpdatedAt;

            if (!string.IsNullOrEmpty(model.Title))
                _Post.Title = model.Title;

            if (!string.IsNullOrEmpty(model.Content))
                _Post.Content = model.Content;

            if (!string.IsNullOrEmpty(model.AuthorId))
                _Post.AuthorId = model.AuthorId;

            if (model.Status is not null && Enum.IsDefined(typeof(PostStatus), model.Status))
                _Post.Status = model.Status.Value;

            if (model.Tags is not null)
            {
                List<Tag> NewTags = new List<Tag>();

                foreach (var tag in model.Tags)
                {
                    var NewTag = await _dbContextIdentity.tags.Where(T => T.Name == tag).FirstOrDefaultAsync();
                    if (NewTag is not null)
                        NewTags.Add(NewTag);
                }
                if (NewTags.Any())
                    _Post.Tags = NewTags;
            }

            if (model.CategoryId is not null)
            {
                var _NewCategory = _dbContextIdentity.categories.Where(C => C.Id == model.CategoryId).FirstOrDefault();
                if (_NewCategory is not null) model.CategoryId = _NewCategory.Id;
            }


            await _blogPosts.UpdateAsync(_Post);

            return Ok(_Post);


        }
    }
}
