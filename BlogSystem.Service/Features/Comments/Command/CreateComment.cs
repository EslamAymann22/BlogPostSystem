using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Repository.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BlogSystem.Service.Features.Comments.Command
{


    public class CreateCommentModel : IRequest<BaseResponse<string>>
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? AuthorId { get; set; }

    }

    public class CreateCommentHandler : BaseResponseHandler, IRequestHandler<CreateCommentModel, BaseResponse<string>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public CreateCommentHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }
        public async Task<BaseResponse<string>> Handle(CreateCommentModel request, CancellationToken cancellationToken)
        {
            var Post = _blogPostDb.blogPosts.FirstOrDefault(p => p.Id == request.PostId);
            if (Post is null)
                return Failed<string>(HttpStatusCode.NotFound, "Post not found");

            var user = await _blogPostDb.Users.Where(U => U.Id == request.AuthorId).FirstOrDefaultAsync();

            if (user is null)
                return Failed<string>(HttpStatusCode.NotFound, "User not found");

            var NewComment = new Comment
            {
                AuthorId = request.AuthorId,
                Content = request.Content,
                PostId = request.PostId,
                CreatedAt = request.CreatedAt
            };

            _blogPostDb.comments.Add(NewComment);
            var result = await _blogPostDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Comment created successfully");
            else
                return Failed<string>(HttpStatusCode.InternalServerError, "Failed to create comment");

        }
    }
}
