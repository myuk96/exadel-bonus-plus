using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ExadelBonusPlus.WebApi
{
    public class GroupingByVersionConvention: IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.ApiExplorer.GroupName = controller.GetApiVersion();
        }
    }
}
