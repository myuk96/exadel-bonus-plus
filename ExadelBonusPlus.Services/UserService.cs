using System;
using System.Threading.Tasks;
using AutoMapper;
using ExadelBonusPlus.Services.Models;
using IdentityModel;
using JWT.Algorithms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JWT.Builder;
using System.Linq;
using ExadelBonusPlus.Services.Properties;

namespace ExadelBonusPlus.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenRefreshService _tokenRefreshService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppJwtSettings _appJwtSettings;
        private readonly IMapper _mapper;
        public UserService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IOptions<AppJwtSettings> appJwtSettings,
            ITokenRefreshService tokenRefreshService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenRefreshService = tokenRefreshService;
            _appJwtSettings = appJwtSettings.Value;
        }
        public async Task<UserInfoDTO> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user is null ? throw new ArgumentException("", Resources.FindbyIdError) : _mapper.Map<UserInfoDTO>(user);
        }
        public async Task<AuthResponce> LogInAsync(LoginUserDTO loginUser, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user is null)
            {
                throw new ArgumentException("", Resources.FindbyIdError);
            }
            if (user.IsActive)
            {
                var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
                if (result.Succeeded)
                {
                    var responce = new AuthResponce();
                    responce.AccessToken = await GetFullJwtAsync(user);
                    var refreshToken = await _tokenRefreshService.GenerateRefreshToken(ipAddress, user.Id);
                    responce.RefreshToken = refreshToken.Token;
                    return responce;
                }
            }
            throw new ArgumentException(Resources.LoginFailed);
        }
        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task RegisterAsync(RegisterUserDTO registerUser)
        {
            if (registerUser is null)
            {
                throw new ArgumentNullException("", Resources.ModelIsNull);
            }
            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
                IsActive = true,
                City = registerUser.City,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                PhoneNumber = registerUser.PhoneNumber
            };
           
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    //Redirect("loginPath")
                }
           
        }
        public async Task<UserInfoDTO> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                throw new ArgumentNullException("", Resources.FindbyIdError);

            }
            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            return result is null ? throw new ArgumentException("", Resources.DeleteError) : _mapper.Map<UserInfoDTO>(user);
        }
        public async Task<UserInfoDTO> RestoreUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                throw new ArgumentNullException("", Resources.FindbyIdError);

            }
            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            return result is null ? throw new ArgumentException("", Resources.DeleteError) : _mapper.Map<UserInfoDTO>(user);

        }
        public async Task<UserInfoDTO> UpdateUserAsync(Guid userId, UpdateUserDTO updateUserDto)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }
            if (updateUserDto is null)
            {
                throw new ArgumentNullException("", Resources.ModelIsNull);
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            user.IsActive = true;
            user.LastName = updateUserDto.LastName;
            user.City = updateUserDto.City;
            user.FirstName = updateUserDto.FirstName;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);

            return result is null ? throw new ArgumentException("", Resources.FindError) : _mapper.Map<UserInfoDTO>(user);
        }
        public async Task<UserInfoDTO> AddRoleToUserAsync(string usersId, string roleName)
        {
            if (usersId is null || roleName is null)
            {
                throw new ArgumentException("", Resources.ModelIsNull);
            }
            var user = await _userManager.FindByIdAsync(usersId);
            if (user is null)
            {
                throw new ArgumentException("", Resources.FindbyIdError);
            }
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                throw new ArgumentException("", Resources.UserInRole);
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result is null ? throw new ArgumentException("", Resources.CreateError) : _mapper.Map<UserInfoDTO>(user);
        }
        public async Task<UserInfoDTO> RemoveUserRoleAsync(string usersId, string roleName)
        {
            if (usersId is null || roleName is null)
            {
                throw new ArgumentException("", Resources.ModelIsNull);
            }

            var user = await _userManager.FindByIdAsync(usersId);
            if (user is null)
            {
                throw new ArgumentException("", Resources.FindbyIdError);
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result is null ? throw new ArgumentException("", Resources.DeleteError) : _mapper.Map<UserInfoDTO>(user);
        }
        public async Task<AuthResponce> RefreshToken(string refreshToken, string ipAddress)
        {
            var tokens = await _tokenRefreshService.GetRefreshTokenByToken(refreshToken);

            var token = tokens.Where(i=>i.CreatedByIp == ipAddress).FirstOrDefault(t => t.IsActive == true);
            if (!(token is null))
            {
                var newRefreshToken = await _tokenRefreshService.UpdateRefreshToken(ipAddress, token);

                var user = await _userManager.FindByIdAsync(newRefreshToken.CreatorId.ToString());
                var jwtToken = await GetFullJwtAsync(user);
                return new AuthResponce
                {
                    RefreshToken = newRefreshToken.Token,
                    AccessToken = jwtToken
                };
            }

            throw new ArgumentNullException("", Resources.FindError);
        }
        private async Task<string> GetFullJwtAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(_appJwtSettings.SecretKey)
                .AddClaim(JwtClaimTypes.Subject, user.Id)
                .AddClaim(JwtClaimTypes.Expiration, DateTimeOffset.UtcNow.AddMinutes(_appJwtSettings.Expiration).ToUnixTimeSeconds())
                .AddClaim(JwtClaimTypes.Email, user.Email)
                .AddClaim(JwtClaimTypes.Audience, _appJwtSettings.Audience)
                .AddClaim(ClaimName.IssuedAt, DateTimeOffset.UtcNow.AddMinutes(_appJwtSettings.Expiration).ToUnixTimeSeconds())
                .AddClaim(JwtClaimTypes.Role, roles)
                .Encode();
        }


    }
}
