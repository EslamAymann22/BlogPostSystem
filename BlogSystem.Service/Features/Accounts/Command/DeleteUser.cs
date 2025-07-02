using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace BlogSystem.Service.Features.Accounts.Command
{

    public class DeleteUserModel : IRequest<BaseResponse<string>>
    {
        public string UserId { get; set; }
    }

    public class DeleteUser : BaseResponseHandler, IRequestHandler<DeleteUserModel, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public DeleteUser(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<BaseResponse<string>> Handle(DeleteUserModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
                return Failed<string>(HttpStatusCode.NotFound, "User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Failed<string>(HttpStatusCode.InternalServerError, "Failed to delete user");
            return Success("User deleted successfully");
        }
    }


}
