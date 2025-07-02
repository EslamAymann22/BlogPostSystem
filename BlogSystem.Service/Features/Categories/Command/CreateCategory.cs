using BlogSystem.Core.Entities;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Repository.Data;
using MediatR;

namespace BlogSystem.Service.Features.Categories.Command
{
    public class CreateCategoryModel : IRequest<BaseResponse<string>>
    {
        public string CategoryName { get; set; }
    }
    public class CreateCategoryHandler : BaseResponseHandler, IRequestHandler<CreateCategoryModel, BaseResponse<string>>
    {
        private readonly BlogPostDbContext _blogPostDb;

        public CreateCategoryHandler(BlogPostDbContext blogPostDb)
        {
            this._blogPostDb = blogPostDb;
        }

        public async Task<BaseResponse<string>> Handle(CreateCategoryModel request, CancellationToken cancellationToken)
        {
            var category = _blogPostDb.categories.FirstOrDefault(T => T.Name == request.CategoryName);
            if (category is null)
            {
                await _blogPostDb.categories.AddAsync(new Category { Name = request.CategoryName });
                await _blogPostDb.SaveChangesAsync();
            }

            return Success("Category Added!");


        }
    }
}
