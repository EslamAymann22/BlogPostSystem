using BlogSystem.Core.ResponseBase.Paginations;
using BlogSystem.Service.Features.Categories.Command;
using BlogSystem.Service.Features.Categories.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {


        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> CreateCategory([FromBody] CreateCategoryModel model)
        {
            return Ok(await _mediator.Send(model));
        }

        public async Task<ActionResult<PaginatedResponse<GetAllCategoriesDto>>> GetAllCategory([FromBody] GetAllCategoriesModel model)
        {
            return Ok(await _mediator.Send(model));
        }
    }
}
