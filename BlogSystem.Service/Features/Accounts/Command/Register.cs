using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using BlogSystem.Core.Services;
using BlogSystem.Service.Features.Accounts.Query;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BlogSystem.Service.Features.Accounts.Command
{

    public class RegisterModel : IRequest<BaseResponse<AccountDto>>
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string DisplayName { get; set; }

    }
    public class RegisterHandler : BaseResponseHandler, IRequestHandler<RegisterModel, BaseResponse<AccountDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public RegisterHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
        }

        public async Task<BaseResponse<AccountDto>> Handle(RegisterModel request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
                return Failed<AccountDto>(HttpStatusCode.Conflict, "This email is already registered");

            if (request.Password != request.ConfirmPassword)
                return Failed<AccountDto>(HttpStatusCode.BadRequest, "Passwords do not match");

            user.Email = request.Email;
            user.UserName = request.Email.Split('@')[0];
            user.DisplayName = request.DisplayName;
            user.Role = UserRole.Reader;

            await _userManager.CreateAsync(user, request.Password);
            return Success(new AccountDto
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }
    }
}
