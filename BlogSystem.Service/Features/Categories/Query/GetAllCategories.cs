using BlogSystem.Core.ResponseBase.Paginations;
using BlogSystem.Repository.Data;
using MediatR;

namespace BlogSystem.Service.Features.Categories.Query
{
    public class GetAllCategoriesDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class GetAllCategoriesModel : PaginationParams, IRequest<PaginatedResponse<GetAllCategoriesDto>>;

    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesModel, PaginatedResponse<GetAllCategoriesDto>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public GetAllCategoriesHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }
        public async Task<PaginatedResponse<GetAllCategoriesDto>> Handle(GetAllCategoriesModel request, CancellationToken cancellationToken)
        {
            var Qdata = _blogPostDb.categories.Select(T => new GetAllCategoriesDto { CategoryId = T.Id, CategoryName = T.Name });
            var Response = await Qdata.ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return Response;
        }
    }
}
