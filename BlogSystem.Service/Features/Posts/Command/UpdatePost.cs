using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Repository.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogSystem.Service.Features.Posts.Command
{
    public class UpdatePostModel : IRequest<BaseResponse<string>>
    {

        [JsonIgnore]
        public string? UserId { get; set; }

        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public string? AuthorId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public PostStatus? Status { get; set; }

        public List<int>? Tags { get; set; } = new List<int>();
        public int? CategoryId { get; set; }
    }
    public class UpdatePostHandler : BaseResponseHandler, IRequestHandler<UpdatePostModel, BaseResponse<string>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public UpdatePostHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }

        public async Task<BaseResponse<string>> Handle(UpdatePostModel request, CancellationToken cancellationToken)
        {
            var post = await _blogPostDb.blogPosts.FirstOrDefaultAsync(P => P.Id == request.Id);
            if (post is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Post not found");

            var user = await _blogPostDb.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);


            if (post.AuthorId != user.Id && user.Role != UserRole.Admin)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not allowed to update this post");

            post.Title = request.Title ?? post.Title;
            post.Content = request.Content ?? post.Content;
            post.UpdatedAt = DateTime.UtcNow;
            post.Status = request.Status ?? post.Status;
            post.CategoryId = request.CategoryId ?? post.CategoryId;
            post.Tags = request.Tags
               .Select(tagId => _blogPostDb.tags.FirstOrDefault(t => t.Id == tagId))
               .Where(tag => tag != null)
               .ToList();

            _blogPostDb.blogPosts.Update(post);
            await _blogPostDb.SaveChangesAsync(cancellationToken);

            return Success("Post updated successfully");

        }
    }
}
