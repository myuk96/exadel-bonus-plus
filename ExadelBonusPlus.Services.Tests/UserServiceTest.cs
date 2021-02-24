using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using AutoMapper;
using ExadelBonusPlus.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SampleDataGenerator;
using Xunit;

namespace ExadelBonusPlus.Services.Tests
{
    public class UserServiceTest
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private UserManager<ApplicationUser> _userManager;
        private List<ApplicationUser> _fakeUsers;


        private Mock<SignInManager<ApplicationUser>> _signManagerMock;
        private SignInManager<ApplicationUser> _signManager;

        private Mock<ITokenRefreshService> _tokenServiceMock;
        private ITokenRefreshService _tokenService;
        private List<TokenRefresh> _fakeTokens;

        private IOptions<AppJwtSettings> _fakeOptions;
        private UserService _userService;
        private IMapper _mapper;
       
        private static Random _random;
         
        
        public UserServiceTest()
        {
        }


        [Fact]
        public async Task<UserInfoDTO> UserInfoDTO_GetUserAsync_Return_UserInfoDTO()
        {
            CreateDefaultApplicationUserServiceInstance();
            var id = _fakeUsers[0].Id;
            var user = await _userService.GetUserAsync(id.ToString());

            Assert.NotNull(user);
            return user;
        }
        [Fact]
        public async Task<UserInfoDTO> UserInfoDTO_RestoreUserAsync_Return_UserInfoDTO()
        {
            CreateDefaultApplicationUserServiceInstance();
            var id = _fakeUsers[0].Id;
            var history = await _userService.RestoreUserAsync(id);

            Assert.NotNull(history);
            return history;
        }
        [Fact]
        public async Task<UserInfoDTO> UserInfoDTO_DeleteUserAsync_Return_UserInfoDTO()
        {
            CreateDefaultApplicationUserServiceInstance();
            var id = _fakeUsers[0].Id;
            var history = await _userService.DeleteUserAsync(id);

            Assert.NotNull(history);
            return history;
        }
        [Fact]
        public async Task<UserInfoDTO> UserInfoDTO_UpdateUserAsync_Return_UserInfoDTO()
        {
            CreateDefaultApplicationUserServiceInstance();
            var user = _mapper.Map<UpdateUserDTO>(_fakeUsers[0]);
            var history = await _userService.UpdateUserAsync(user.Id, user);

            Assert.NotNull(history);
            return history;
        }

        [Fact]
        public async Task<UserInfoDTO> UserInfoDTO_AddRoleToUserAsync_Return_UserInfoDTO()
        {
            CreateDefaultApplicationUserServiceInstance();
            var user = _mapper.Map<UpdateUserDTO>(_fakeUsers[0]);
            var history = await _userService.AddRoleToUserAsync(user.Id.ToString(), RandomString(5));

            Assert.NotNull(history);
            return history;
        }
        [Fact]
        public async Task<UserInfoDTO> UserInfoDTO_RemoveUserRoleAsync_Return_UserInfoDTO()
        {
            CreateDefaultApplicationUserServiceInstance();
            var user = _mapper.Map<UpdateUserDTO>(_fakeUsers[0]);
            var history = await _userService.RemoveUserRoleAsync(user.Id.ToString(), RandomString(5));

            Assert.NotNull(history);
            return history;
        }
        [Fact]
        public async Task<AuthResponce> LoginUserDTO_LogInAsync_Return_AuthResponce()
        {
            CreateDefaultApplicationUserServiceInstance();
            var _fake = _fakeUsers[0];
            var fakeLoginModel = new LoginUserDTO();
            fakeLoginModel.Email = _fake.Email;
            fakeLoginModel.Password = RandomString(5);
            var authResponce = await _userService.LogInAsync(fakeLoginModel, RandomString(9));

            Assert.NotNull(authResponce);
            return authResponce;
        }
        [Fact]
        public async Task<AuthResponce> String_RefreshToken_Return_AuthResponce()
        {
            CreateDefaultApplicationUserServiceInstance();
            var authResponce = await _userService.RefreshToken(RandomString(10), _fakeTokens[0].CreatedByIp);

            Assert.NotNull(authResponce);
            return authResponce;
        }




        Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : MongoIdentityUser<Guid>
        {
            return new Mock<UserManager<TIDentityUser>>(
                new Mock<IUserStore<TIDentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<TIDentityUser>>().Object,
                new IUserValidator<TIDentityUser>[0],
                new IPasswordValidator<TIDentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<TIDentityUser>>>().Object);

        }
        Mock<SignInManager<TIDentityUser>> GetSignInManagerMock<TIDentityUser>(Mock<UserManager<TIDentityUser>> _userManagerMock) where TIDentityUser : MongoIdentityUser<Guid>
        {
            return new Mock<SignInManager<TIDentityUser>>(
                _userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TIDentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<TIDentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<TIDentityUser>>().Object
            );

        }
        private void CreateDefaultApplicationUserServiceInstance()
        {
            var services = new ServiceCollection();

            var mockStore = new Mock<IUserStore<ApplicationUser>>();
            var mockOptionsAccessor = new Mock<IOptions<IdentityOptions>>();
            var mockPasswordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var validators = new List<IPasswordValidator<ApplicationUser>>();
            var mockKeyNormalizer = new Mock<ILookupNormalizer>();
            var mockErrors = new Mock<IdentityErrorDescriber>();
            var mockServices = new Mock<IServiceProvider>();
            var mockLogger = new Mock<ILogger<UserManager<ApplicationUser>>>();

            var userManager = new UserManager<ApplicationUser>(mockStore.Object,
                mockOptionsAccessor.Object,
                mockPasswordHasher.Object,
                userValidators,
                validators,
                mockKeyNormalizer.Object,
                mockErrors.Object,
                mockServices.Object,
                mockLogger.Object);

            var userResolver = new UserResolver(userManager, _mapper);

            var bonusResolver = new BonusResolver(new Mock<IVendorService>().Object, new Mock<IBonusService>().Object, _mapper);

            services.AddTransient(sp => userManager);
            services.AddTransient(sp => userResolver);
            services.AddTransient(sp => bonusResolver);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(myProfile);
                cfg.ConstructServicesUsing(serviceProvider.GetService);
            });

            _mapper = new Mapper(configuration);
            _random = new Random();
            GenerateData();
            CreateMock();
            _userManager = _userManagerMock.Object;
            _signManager = _signManagerMock.Object;
            _tokenService = _tokenServiceMock.Object;
            _userService = new UserService(_signManager, _userManager, _mapper, _fakeOptions, _tokenService);
        }
        private void CreateMock()
        {
            _userManagerMock = GetUserManagerMock<ApplicationUser>();
            _userManagerMock.Setup(s => s.FindByIdAsync(It.IsAny<String>())).Returns(Task.FromResult(new ApplicationUser()));
            _userManagerMock.Setup(s => s.FindByNameAsync(It.IsAny<String>())).Returns(Task.FromResult(_fakeUsers[0]));
            _userManagerMock.Setup(s => s.FindByEmailAsync(It.IsAny<String>())).Returns(Task.FromResult(_fakeUsers[0]));

            _userManagerMock.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(s => s.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(s => s.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(s => s.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<String>())).ReturnsAsync(false);
            _userManagerMock.Setup(s => s.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<String>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(s => s.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<String>())).ReturnsAsync(IdentityResult.Success); ;

            _signManagerMock = GetSignInManagerMock(_userManagerMock);
            _signManagerMock.Setup(s => s.PasswordSignInAsync(It.IsAny<String>(), It.IsAny<String>(), false, true))
                .ReturnsAsync(SignInResult.Success);
            _signManagerMock.Setup(s => s.SignOutAsync());


            _tokenServiceMock = new Mock<ITokenRefreshService>();
            _tokenServiceMock.Setup(s => s.GenerateRefreshToken(It.IsAny<String>(), It.IsAny<Guid>())).ReturnsAsync(_fakeTokens[0]);
            _tokenServiceMock.Setup(s => s.GetRefreshTokenByToken(It.IsAny<String>())).ReturnsAsync(_fakeTokens);
            _tokenServiceMock.Setup(s => s.UpdateRefreshToken(It.IsAny<String>(), It.IsAny<TokenRefresh>())).ReturnsAsync(_fakeTokens[0]);
            
        }
        private void GenerateData()
        {
            var historyGenerator = Generator
                    .For<ApplicationUser>()
                    .For(x => x.Id)
                    .ChooseFrom(Guid.NewGuid())
                    .For(x => x.City)
                    .ChooseFrom(RandomString(10))
                    .For(x => x.IsActive)
                    .ChooseFrom(true)
                    .For(x => x.City)
                    .ChooseFrom(RandomString(10))
                    .For(x => x.PhoneNumber)
                    .ChooseFrom(RandomString(7))
                    .For(x => x.LastName)
                    .ChooseFrom(RandomString(10))
                    .For(x => x.Email)
                    .ChooseFrom(RandomString(5) + ("@gmail.com"))
                    .For(x => x.FirstName)
                    .ChooseFrom(RandomString(10));
            _fakeUsers = historyGenerator.Generate(10).ToList();

            var tokenGenerator = Generator
                .For<TokenRefresh>()
                .For(x => x.Id)
                .ChooseFrom(Guid.NewGuid())
                .For(x => x.CreatorId)
                .ChooseFrom(Guid.NewGuid())
                .For(x => x.Token)
                .ChooseFrom(RandomString(20))
                .For(x => x.CreatedDate)
                .ChooseFrom(DateTime.Now)
                .For(x=>x.Expires)
                .ChooseFrom(DateTime.Now.AddDays(5));
            _fakeTokens = tokenGenerator.Generate(10).ToList();


            AppJwtSettings _fakeAppJwtSettings;
            var SettingsGenerator = Generator
                .For<AppJwtSettings>()
                .For(x => x.Audience)
                .ChooseFrom(RandomString(10))
                .For(x => x.Expiration)
                .ChooseFrom(15)
                .For(x => x.Issuer)
                .ChooseFrom(RandomString(10))
                .For(x => x.SecretKey)
                .ChooseFrom(RandomString(10));
            _fakeAppJwtSettings = SettingsGenerator.Generate(1).FirstOrDefault();

            
            _fakeOptions = Options.Create(_fakeAppJwtSettings);
        }
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
