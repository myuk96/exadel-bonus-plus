using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ExadelBonusPlus.WebApi
{
    public static class ApiControllerExtensions
    {
        public static string GetApiVersion(this ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiVersion = controllerNamespace.Split(".").Last().ToLower();
            if (!apiVersion.StartsWith("v"))
            {
                apiVersion = "v1";
            }

            return apiVersion;
        }
    }
}
