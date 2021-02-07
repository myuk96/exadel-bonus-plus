﻿using System.Collections.Generic;
using AutoMapper;
using ExadelBonusPlus.Services.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace ExadelBonusPlus.Services.Models
{
    public class UserResolver : IValueResolver<ApplicationUser, UserInfoDTO, IList<string>>
    { 
        private readonly UserManager<ApplicationUser> _userManager;
        public UserResolver(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<string> Resolve(ApplicationUser source, UserInfoDTO destination, IList<string> destMember, ResolutionContext context)
        {
            var role = _userManager.GetRolesAsync(source).Result;
            return role;
        }

    }
}