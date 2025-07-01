using BlogSystem.Core.Entities;
using BlogSystem.Repository.Data;
using BlogSystem.Service.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogSystem.Service.Features.Comments.Command
{



    public class DeleteCommentByIdModel : IRequest<BaseResponse<string>>
    {
        public string? UserEmal { get; set; }
        public int CommentId { get; set; }
    }

    public class DeleteCommentByIdHandler : BaseResponseHandler, IRequestHandler<DeleteCommentByIdModel, BaseResponse<string>>
    {
        private readonly BlogPostDbContext _blogPostDb;
        public DeleteCommentByIdHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }
        public async Task<BaseResponse<string>> Handle(DeleteCommentByIdModel request, CancellationToken cancellationToken)
        {
            var comment = await _blogPostDb.comments.FindAsync(request.CommentId);
            if (comment is null)
                return Failed<string>(HttpStatusCode.NotFound, "This comment is not found");

            var user = await _blogPostDb.Users.Where(U => U.Email == request.UserEmal).FirstOrDefaultAsync();
            if ((user is null) || (user.Id != comment.AuthorId && user.Role != UserRole.Admin))
                return Failed<string>(HttpStatusCode.Unauthorized, "You are not authorized to delete this comment");

            _blogPostDb.comments.Remove(comment);
            await _blogPostDb.SaveChangesAsync(cancellationToken);
            return Success("Comment deleted successfully");
        }
    }
    {
    }
}
