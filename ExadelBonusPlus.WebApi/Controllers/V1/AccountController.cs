using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ExadelBonusPlus.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExadelBonusPlus.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Register(RegisterUserDTO registerUser)
        {
            await _userService.RegisterAsync(registerUser);
            return Ok();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get info about authorized user", Type = typeof(ResultDto<UserInfoDTO>))]
        
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _userService.LogInAsync(loginUser, ipAddress);
            setTokenCookie(result.RefreshToken);
            return Ok(result.AccessToken);
        }

        [HttpPost]
        [Route("logout")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogOutAsync();
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }

        [HttpGet]
        [Route("getInfo")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get info about authorized user", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> GetUserInfoAsync()
        {
            var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.GetUserAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("tokenrefresh")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "refresh your token", Type = typeof(ResultDto<AuthResponce>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponce>> GetRefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _userService.RefreshToken(refreshToken, ipAddress);
            if (result == null)
                return Unauthorized(new { message = "Invalid token" });
            setTokenCookie(result.RefreshToken);
            return Ok(result.AccessToken);
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(2),
                SameSite =  SameSiteMode.None

            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        
    }
}
