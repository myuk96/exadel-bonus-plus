using System;
using System.Text;
using ExadelBonusPlus.DataAccess;
using ExadelBonusPlus.Services;
using ExadelBonusPlus.Services.Models;
using ExadelBonusPlus.Services.Models.DTOValidator;
using ExadelBonusPlus.Services.Models.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ExadelBonusPlus.WebApi
{
    public static class IServiceCollectionExtensions
    {
        public static void AddBonusTransient(this IServiceCollection services)
        {
            services.AddTransient<IBonusRepository, BonusRepository>();
            services.AddTransient<IBonusService, BonusService>();

            services.AddTransient<IValidator<AddBonusDto>, AddBonusDtoValidator>();
            services.AddTransient<IValidator<UpdateBonusDto>, UpdateBonusDtoValidator>();



        }

        public static void AddHistoryTransient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHistoryRepository, HistoryRepository>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.Configure<EmailSettings>(configuration.GetSection(
                nameof(EmailSettings)));
            services.AddScoped<IEmailService, EmailServices>();
        }

        public static void AddVendorTransient(this IServiceCollection services)
        {
            services.AddTransient<IVendorService, VendorService>();
            services.AddTransient<IVendorRepository, VendorRepository>();

            services.AddTransient<IValidator<AddVendorDto>, AddVendorDtoValidator>();
            services.AddTransient<IValidator<VendorDto>, VendorDtoValidator>();
        }

        public static void AddApiIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IValidator<RegisterUserDTO>, RegisterUserValidator>();
            services.AddTransient<IValidator<LoginUserDTO>, LoginUserValidator>();
            services.AddTransient<IValidator<UpdateUserDTO>, UpdateUserValidator>();
            services.AddTransient<IRoleService, RoleService>();
            
            services.AddTransient<ITokenRefreshRepository, TokenRefreshRepository>();
            services.AddTransient<ITokenRefreshService, TokenRefreshService>();


            services.Configure<AppJwtSettings>(configuration.GetSection(
                nameof(AppJwtSettings)));

            services.ConfigureApplicationCookie(options => {
                options.Cookie.SameSite = SameSiteMode.None;
            });


            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
                    configuration["MongoDbSettings:ConnectionString"],
                    configuration["MongoDbSettings:DatabaseName"])
                .AddDefaultTokenProviders();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });

            });
           

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidIssuer = configuration["AppJwtSettings:Issuer"],
                        ValidateAudience = false,
                        ValidAudience = configuration["AppJwtSettings:Audience"],
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppJwtSettings:SecretKey"]))
                    };
                });
        }
    }
}
