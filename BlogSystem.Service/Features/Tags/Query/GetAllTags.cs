using BlogSystem.Core.ResponseBase.Paginations;
using BlogSystem.Repository.Data;
using MediatR;

namespace BlogSystem.Service.Features.Tags.Query
{
    public class GetAllTagsDto
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
    public class GetAllTagsModel : PaginationParams, IRequest<PaginatedResponse<GetAllTagsDto>>;

    public class GetAllTagsHandler : IRequestHandler<GetAllTagsModel, PaginatedResponse<GetAllTagsDto>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public GetAllTagsHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }
        public async Task<PaginatedResponse<GetAllTagsDto>> Handle(GetAllTagsModel request, CancellationToken cancellationToken)
        {
            var Qdata = _blogPostDb.tags.Select(T => new GetAllTagsDto { TagId = T.Id, TagName = T.Name });
            var Response = await Qdata.ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return Response;
        }
    }
}
