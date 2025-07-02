using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.Paginations;
using BlogSystem.Repository.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Service.Features.Posts.Query
{
    public class GetPostsDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        public List<string> Tags { get; set; }
        [Required]
        public string Category { get; set; }

    }

    public class GetAllPostsModel : PaginationParams, IRequest<PaginatedResponse<GetPostsDto>>;

    public class GetAllPostsHandler : IRequestHandler<GetAllPostsModel, PaginatedResponse<GetPostsDto>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public GetAllPostsHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }

        public async Task<PaginatedResponse<GetPostsDto>> Handle(GetAllPostsModel request, CancellationToken cancellationToken)
        {


            var Qdata = _blogPostDb.blogPosts.Where(P => P.Status == PostStatus.Published)
                .Include(P => P.Author).Include(P => P.Category)
                .Select(P => new GetPostsDto
                {
                    AuthorId = P.AuthorId,
                    AuthorName = P.Author.UserName,
                    Category = P.Category.Name,
                    Content = P.Content,
                    CreatedAt = P.CreatedAt,
                    Id = P.Id,
                    Tags = P.Tags.Select(T => T.Name).ToList(),
                    Title = P.Title,
                    UpdatedAt = P.UpdatedAt
                });

            var result = await Qdata.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return result;

        }
    }

}
