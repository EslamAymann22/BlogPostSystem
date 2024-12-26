using BlogSystem.Core.Entities;
using BlogSystem.Core.Repositories;
using BlogSystem.Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.APIs.Controllers
{
    
    public class PostController : ApiBaseController
    {
        private readonly IGenericRepository<Post> _blogPosts;

        public PostController(IGenericRepository<Post> blogPosts)
        {
            _blogPosts = blogPosts;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPostsAsync()
        {
            var Spec = new PostSpecificationWithAllIncludes();

            var Result = await _blogPosts.GetAllSpecAsync(Spec);
            //var Result = await _blogPosts.GetAllAsync();
            return Ok(Result);
        }

        [HttpGet("{id}")]
        // BaseUrl/Api/post/id(int)
        public async Task<ActionResult<Post>> GetPostByIdAsync(int id)
        {
            var Spec = new PostSpecificationWithAllIncludes(id);
            //var Result =await _blogPosts.GetByIdAsync(id);
            var Result =await _blogPosts.GetByIdSpecAsync(Spec);
            return Ok(Result);
        }
    }
} 
