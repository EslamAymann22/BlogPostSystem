using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace BlogSystem.Service.Features.Accounts.Query
{
    public class AccountDto
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
    }


    public class LoginModel : IRequest<BaseResponse<AccountDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }


    }


    public class LoginHandler : BaseResponseHandler, IRequestHandler<LoginModel, BaseResponse<AccountDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }


        public async Task<BaseResponse<AccountDto>> Handle(LoginModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            bool IsValid = true;
            if (user is null)
                IsValid = false;

            IsValid &= await _userManager.CheckPasswordAsync(user, request.Password);

            if (!IsValid)
                return Failed<AccountDto>(HttpStatusCode.NotFound, "Email or password is wrong");

            var Token = await _tokenService.CreateTokenAsync(user, _userManager);

            var response = new AccountDto
            {
                DisplayName = user.DisplayName,
                Role = user.Role.ToString(),
                Token = Token,
                UserId = user.Id,
                Email = user.Email
            };

            return Success(response);

        }
    }

}
