using AutoMapper;
using BlogSystem.APIs.DTOs;
using BlogSystem.APIs.Errors;
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
        private readonly IMapper _mapper;

        public PostController(IGenericRepository<Post> blogPosts,IMapper mapper)
        {
            _blogPosts = blogPosts;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDtoToReturn>>> GetAllPostsAsync([FromQuery] PostSpecificationParams Parms )
        {
            //if(Parms.status == PostStatus.Published) { Parms.status = null; }
            var Spec = new PostSpecificationWithAllIncludes(Parms);
            var Result = await _blogPosts.GetAllSpecAsync(Spec);
            var ResultDto = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDtoToReturn>>(Result);
            //var Result = await _blogPosts.GetAllAsync();
            return Ok(ResultDto);
        }

        [HttpGet("{id}")]
        // BaseUrl/Api/post/id(int)
        public async Task<ActionResult<PostDtoToReturn>> GetPostByIdAsync(int id)
        {
            var Spec = new PostSpecificationWithAllIncludes(id);
            //var Result =await _blogPosts.GetByIdAsync(id);
            var Result =await _blogPosts.GetByIdSpecAsync(Spec);
            var ResultDto = _mapper.Map<Post, PostDtoToReturn>(Result);
            if (ResultDto == null) return NotFound(new ApiResponse(404));
            return Ok(ResultDto);
        }

        //private void MakeServerError()
        //{
        //    string s = null;
        //    int.Parse(s);
        //}
    }
} 
