using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace BlogSystem.Service.Features.Role.Command
{
    public class ChangeUserRoleModel : IRequest<BaseResponse<string>>
    {
        public string UserId { get; set; }
        public UserRole userRole { get; set; }
    }

    public class ChangeUserRoleHandler : BaseResponseHandler, IRequestHandler<ChangeUserRoleModel, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public ChangeUserRoleHandler(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<BaseResponse<string>> Handle(ChangeUserRoleModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Failed<string>(HttpStatusCode.NotFound, "User is not found");

            await _userManager.AddToRoleAsync(user, request.userRole.ToString());

            return Success("Role added successfully");

        }
    }

}


