using BlogSystem.Repository.Data;
using BlogSystem.Service.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogSystem.Service.Features.Comments.Query
{
    public class GetCommentDto
    {
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
    }
    public class GetCommentByIdModel : IRequest<BaseResponse<GetCommentDto>>
    {
        public int CommentId { get; set; }
    }

    public class GetCommentByIdHandler : BaseResponseHandler, IRequestHandler<GetCommentByIdModel, BaseResponse<GetCommentDto>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public GetCommentByIdHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }

        public async Task<BaseResponse<GetCommentDto>> Handle(GetCommentByIdModel request, CancellationToken cancellationToken)
        {
            var Comment = _blogPostDb.comments
                .Where(c => c.Id == request.CommentId)
                .Include(C => C.Author)
                .Select(c => new GetCommentDto
                {
                    AuthorName = c.Author.DisplayName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    PostId = c.PostId
                })
                .FirstOrDefault();
            if (Comment is null)
                return Failed<GetCommentDto>(HttpStatusCode.NotFound, "This comment is not found");

            return Success(Comment);

        }
    }

}
