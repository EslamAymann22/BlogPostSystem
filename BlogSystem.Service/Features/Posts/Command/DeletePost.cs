using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Repository.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Service.Features.Posts.Command
{
    public class DeletePostModel : IRequest<BaseResponse<string>>
    {
        public string? UserEmail { get; set; }
        public int PostId { get; set; }
    }
    public class DeletePostHandler : BaseResponseHandler, IRequestHandler<DeletePostModel, BaseResponse<string>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public DeletePostHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }
        public async Task<BaseResponse<string>> Handle(DeletePostModel request, CancellationToken cancellationToken)
        {


            var post = await _blogPostDb.blogPosts.FindAsync(request.PostId);

            if (post is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Post not found");


            var User = await _blogPostDb.Users.FirstOrDefaultAsync(u => u.Email == request.UserEmail, cancellationToken);
            if (User is null || (post.AuthorId != User.Id && User.Role != UserRole.Admin))
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not allowed to delete this post");

            _blogPostDb.blogPosts.Remove(post);
            await _blogPostDb.SaveChangesAsync(cancellationToken);
            return Success("Post deleted successfully");

        }
    }


}
