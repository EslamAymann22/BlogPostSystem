using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Repository.Data;
using MediatR;
using System.Net;

namespace BlogSystem.Service.Features.Posts.Command
{
    public class CreatePostModel : IRequest<BaseResponse<string>>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorEmail { get; set; }
        public PostStatus Status { get; set; }
        public List<int> Tags { get; set; }
        public int CategoryId { get; set; }
    }

    public class CreasePostHandler : BaseResponseHandler, IRequestHandler<CreatePostModel, BaseResponse<string>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public CreasePostHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }
        public async Task<BaseResponse<string>> Handle(CreatePostModel request, CancellationToken cancellationToken)
        {

            var user = _blogPostDb.Users.FirstOrDefault(u => u.Email == request.AuthorEmail);
            if (user is null)
                return Failed<string>(HttpStatusCode.Unauthorized, "User not found");

            var post = new Post
            {
                AuthorId = user.Id,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                Status = request.Status,
                CategoryId = request.CategoryId,
                Tags = request.Tags
                    .Select(tagId => _blogPostDb.tags.FirstOrDefault(t => t.Id == tagId)).ToList(),
                UpdatedAt = DateTime.UtcNow
            };

            _blogPostDb.blogPosts.Add(post);
            await _blogPostDb.SaveChangesAsync(cancellationToken);
            return Created("Post created successfully");
        }
    }

}
