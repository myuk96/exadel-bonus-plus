﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ExadelBonusPlus.WebApi.ViewModel
{
    public class AuthResponce
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
