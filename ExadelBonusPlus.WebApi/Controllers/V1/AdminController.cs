using System;
using System.Net;
using System.Threading.Tasks;
using ExadelBonusPlus.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExadelBonusPlus.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AdminController(IUserService userService,
                                IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        [HttpDelete]
        [Route("user/{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Delete user", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> DeleteUserAsync([FromRoute] Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("user/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Restore user", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> RestoreUser([FromRoute] Guid id)
        {
            var result = await _userService.RestoreUserAsync(id);
            return Ok(result);
        }
        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("user/{id}/addroles")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Add role to user", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> AddRoleToUserAsync([FromRoute] Guid id,  string roleName)
        {
            var result = await _userService.AddRoleToUserAsync(id.ToString(), roleName);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("user/{id}/removeroles")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Remove role from user", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> RemoveUserRoleAsync([FromRoute] Guid id,  string roleName)
        {
            var result = await _userService.AddRoleToUserAsync(id.ToString(), roleName);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("user/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Update user info", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> UdpateUserAsync([FromRoute] Guid id, [FromBody] UpdateUserDTO updateUserDto)
        {
            var result = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("user/{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get info about user",
            Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> GetUserByIdAsync([FromRoute] Guid id)
        {
            var result = await _userService.GetUserAsync(id.ToString());
            return Ok(result);
        }

        [HttpDelete]
        [Route("role")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Delete role", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> DeleteRoleAsync(Guid id)
        {
            var result = await _roleService.DeleteRole(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("role")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Create role", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> CreateRoleAsync(string roleName)
        {
            var result = await _roleService.AddRole(roleName);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("role/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Update role ", Type = typeof(ResultDto<UserInfoDTO>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> UdpateRoleAsync([FromRoute] Guid id , string roleName)
        {
            var result = await _roleService.UpdateRole(id, roleName);
            return Ok(result);
        }
    }
}
