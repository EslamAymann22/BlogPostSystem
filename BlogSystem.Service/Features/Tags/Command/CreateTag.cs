using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Repository.Data;
using MediatR;
namespace BlogSystem.Service.Features.Tags.Command
{
    public class CreateTagModel : IRequest<BaseResponse<string>>
    {
        public string TagName { get; set; }
    }
    public class CreateTagHandler : BaseResponseHandler, IRequestHandler<CreateTagModel, BaseResponse<string>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public CreateTagHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }

        public async Task<BaseResponse<string>> Handle(CreateTagModel request, CancellationToken cancellationToken)
        {
            var Tag = _blogPostDb.tags.FirstOrDefault(T => T.Name == request.TagName);
            if (Tag is null)
            {
                await _blogPostDb.tags.AddAsync(new Tag { Name = request.TagName });
                await _blogPostDb.SaveChangesAsync();
            }

            return Success("Tag Added!");


        }
    }
}
