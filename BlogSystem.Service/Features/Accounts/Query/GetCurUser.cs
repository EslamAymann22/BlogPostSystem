using BlogSystem.Core.Entities.Identity;
using BlogSystem.Service.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace BlogSystem.Service.Features.Accounts.Query
{

    public class GetCurUserModel : IRequest<BaseResponse<AccountDto>>
    {
        public string Email { get; set; }
    }


    public class GetCurUserHandler : BaseResponseHandler, IRequestHandler<GetCurUserModel, BaseResponse<AccountDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetCurUserHandler(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<BaseResponse<AccountDto>> Handle(GetCurUserModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Failed<AccountDto>(HttpStatusCode.NotFound, "User not found");
            return Success(new AccountDto
            {
                DisplayName = user.DisplayName,
                Token = null,
                UserId = user.Id,
                Role = user.Role.ToString(),
            });
        }
    }
}
