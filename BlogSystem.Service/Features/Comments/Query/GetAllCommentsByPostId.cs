using BlogSystem.Core.ResponseBase.Paginations;
using BlogSystem.Repository.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Service.Features.Comments.Query
{


    public class GetAllCommentsByPostIdModel : PaginationParams, IRequest<PaginatedResponse<GetCommentDto>>
    {
        public int PostId { get; set; }
    }

    public class GetAllCommentsByPostId : IRequestHandler<GetAllCommentsByPostIdModel, PaginatedResponse<GetCommentDto>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public GetAllCommentsByPostId(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }
        public async Task<PaginatedResponse<GetCommentDto>> Handle(GetAllCommentsByPostIdModel request, CancellationToken cancellationToken)
        {
            var Qdata = _blogPostDb.comments
                .Where(C => C.PostId == request.PostId).Include(c => c.Author)
                .Select(c => new GetCommentDto
                {
                    AuthorName = c.Author.DisplayName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    PostId = c.PostId
                });
            var Response = await Qdata.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return Response;
        }
    }

}
