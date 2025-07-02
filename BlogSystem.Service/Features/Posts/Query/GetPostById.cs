using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Repository.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogSystem.Service.Features.Posts.Query
{
    public class GetPostByIdModel : IRequest<BaseResponse<GetPostsDto>>
    {
        public int PostId { get; set; }
        public string? UserEmail { get; set; }
    }

    public class GetPostByIdHandler : BaseResponseHandler, IRequestHandler<GetPostByIdModel, BaseResponse<GetPostsDto>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public GetPostByIdHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }

        public async Task<BaseResponse<GetPostsDto>> Handle(GetPostByIdModel request, CancellationToken cancellationToken)
        {
            var post = await _blogPostDb.blogPosts
                .Where(P => P.Id == request.PostId)
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync();

            if (post is null)
                return Failed<GetPostsDto>(HttpStatusCode.NotFound, "Post not found");

            var user = await _blogPostDb.Users
                .FirstOrDefaultAsync(u => u.Email == request.UserEmail, cancellationToken);

            if (post.Status != PostStatus.Published && (post.AuthorId != user.Id && user.Role != UserRole.Admin))
                return Failed<GetPostsDto>(HttpStatusCode.Unauthorized, "You are not allowed to view this post");

            var result = new GetPostsDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorId = post.AuthorId,
                AuthorName = post.Author.UserName,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                Tags = post.Tags.Select(t => t.Name).ToList(),
                Category = post.Category.Name
            };
            return Success(result);
        }
    }
}
