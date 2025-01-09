using BlogSystem.APIs.DTOs;
using BlogSystem.APIs.Errors;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogSystem.APIs.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager
            , ITokenService tokenService
            )
        {
            _userManager = userManager;
            this._tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto LoginData)
        {

            var _Account = await _userManager.FindByEmailAsync(LoginData.Email);
            if (_Account == null) return NotFound(new ApiResponse(404, "This Email is not found"));

            var _Check = await _userManager.CheckPasswordAsync(_Account, LoginData.Password);

            if (!_Check) return NotFound(new ApiResponse(404, "incorrect Password !!!"));

            var _ReturnedAccount = new UserDto()
            {
                DisplayName = _Account.DisplayName,
                Email = _Account.Email,
                Token = await _tokenService.CreateTokenAsync(_Account, _userManager)
            };
            return Ok(_ReturnedAccount);
            //return Ok(new UserDto() { DisplayName = LoginData.Email.Split('@')[0], Email = LoginData.Email, Token = "WEFR@$%G34rh87fr359gr983487@$%%@##F$$#$%$524RTF46453rFw" });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto _RegisterData)
        {
            var _Account = await _userManager.FindByEmailAsync(_RegisterData.Email);

            if (_Account is not null) return NotFound(new ApiResponse(400, "This User Already Used"));
            var _User = new AppUser()
            {
                Email = _RegisterData.Email,
                DisplayName = _RegisterData.DisplayName,
                UserName = _RegisterData.Email.Split('@')[0]
            };
            var _NewUserResult = await _userManager.CreateAsync(_User, _RegisterData.Password);
            if (!_NewUserResult.Succeeded) return BadRequest(new ApiResponse(400));

            var _ReturnedUser = new UserDto()
            {
                DisplayName = _RegisterData.DisplayName,
                Token = await _tokenService.CreateTokenAsync(_User, _userManager),
                Email = _RegisterData.Email,
            };
            return Ok(_ReturnedUser);
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurUser()
        {

            var _Email =  User.FindFirstValue(ClaimTypes.Email);
            var _Account = await _userManager.FindByEmailAsync(_Email);

            UserDto _ReturnedUser = new UserDto()
            {
                Email = _Account.Email,
                DisplayName = _Account.DisplayName,
                Token =await _tokenService.CreateTokenAsync(_Account, _userManager)
            };
            return Ok(_ReturnedUser);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = ("Admin"))]
        public async Task<ActionResult<UserDto>> DeleteUser(string Id)
        {

            var _User = await _userManager.Users.Where(U => U.Id == Id).FirstOrDefaultAsync();
            if (_User is null)
                return NotFound(new ApiResponse(404, "This user id not found!!"));
            //var User =await _dbContextIdentity.Users.Where(U=>U.Id==Id).FirstOrDefaultAsync();
            var Result = await _userManager.DeleteAsync(_User);
            if (!Result.Succeeded)
                return BadRequest(Result);  /// i don't know what will return for me but it's not Error ^^
            return Ok(new UserDto() { DisplayName = _User.DisplayName, Email = _User.Email });
        }


    }
}
